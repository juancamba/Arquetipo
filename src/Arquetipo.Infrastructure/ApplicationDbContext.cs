using Arquetipo.Application.Exceptions;
using Arquetipo.Domain.Abstractions;
using Arquetipo.Domain.Users;
using Arquetipo.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
namespace Arquetipo.Infrastructure;

public sealed class ApplicationDbContext : DbContext, IUnitOfWork
{
     public DbSet<User> Users { get; set; }
     public DbSet<OutboxMessage> OutboxMessages { get; set; }

     private static readonly JsonSerializerSettings jsonSerializerSettings = new()
    {
        TypeNameHandling  = TypeNameHandling.All
    };
     
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)  {  }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            AddDomainEventsToOutboxMessages();
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException("La excepcion por concurrencia se disparo", ex);
        }
    }

    private void AddDomainEventsToOutboxMessages()
    {
        var outboxMessages = ChangeTracker
            .Entries<IEntity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                var domainEvents = entity.GetDomainEvents();
                entity.ClearDomainEvents();
                return domainEvents;
            }).Select(domainEvent=> new OutboxMessage(
                Guid.NewGuid(), 
                DateTime.UtcNow,
                domainEvent.GetType().Name,
                JsonConvert.SerializeObject(domainEvent,jsonSerializerSettings)
            ))
            .ToList();
        
        AddRange(outboxMessages);
    }
}