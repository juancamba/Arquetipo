using Arquetipo.Domain.Abstractions;

using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;

namespace Arquetipo.Infrastructure.Outbox;

[DisallowConcurrentExecution]
internal sealed class InvokeOutboxMessagesJob : IJob
{
    private static readonly JsonSerializerSettings jsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All
    };

    private readonly ApplicationDbContext _dbContext;
    private readonly IPublisher _publisher; //usamos el publisher para publicar el mensaje y enviarlo al rabbit o email en este caso
    
    private readonly OutboxOptions _outboxOptions;
    private readonly ILogger<InvokeOutboxMessagesJob> _logger;
    
    public InvokeOutboxMessagesJob(
        ApplicationDbContext dbContext,
        IPublisher publisher,
        IOptions<OutboxOptions> outboxOptions,
        ILogger<InvokeOutboxMessagesJob> logger
    )
    {
        _dbContext = dbContext;
        _publisher = publisher;
        _outboxOptions = outboxOptions.Value;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Iniciando el proceso de outbox messages");

        // Obtener los mensajes sin procesar, ordenados por fecha y limitados al batch size
        var records = await _dbContext.OutboxMessages
            .Where(m => m.ProcessedOnUtc == null)
            .OrderBy(m => m.OcurredOnUtc)
            .Take(_outboxOptions.BatchSize)
            .ToListAsync(context.CancellationToken);

        if (!records.Any())
        {
            _logger.LogInformation("No hay mensajes de outbox pendientes de procesar");
            return;
        }

        // Usar transacción para procesar los mensajes
        using var transaction = await _dbContext.Database.BeginTransactionAsync(context.CancellationToken);

        try
        {
            foreach (var message in records)
            {
                Exception? exception = null;
                try
                {
                    // el IDomainEvent, es reemplazado por el usuario, vehiculo o alquiler en tiempo de ejecución
                    var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(
                        message.Content,
                        jsonSerializerSettings
                    )!;
                    
                    // publicamos el evento de dominio: usuario, vehiculo o alquiler
                    await _publisher.Publish(domainEvent, context.CancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Se produjo una excepcion en el outbox message {MessageId}", message.Id
                    );

                    exception = ex;
                }

                // Actualizar el mensaje con la fecha de procesamiento y error si aplica
                message.MarkAsProcessed(exception);
            }

            // Guardar todos los cambios en una sola transacción
            await _dbContext.SaveChangesAsync(context.CancellationToken);
            await transaction.CommitAsync(context.CancellationToken);
            
            _logger.LogInformation("Se ha completado el procesamiento de {MessageCount} mensajes de outbox", records.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error durante el procesamiento de mensajes de outbox");
            await transaction.RollbackAsync(context.CancellationToken);
            throw new InvalidOperationException(
                "Ocurrió un error durante el procesamiento de los mensajes del outbox.",
                ex
            );
        }
    }
}
