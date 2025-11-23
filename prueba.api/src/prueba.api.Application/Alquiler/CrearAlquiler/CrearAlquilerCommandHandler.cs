using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mediator;
using Microsoft.Extensions.Logging;


namespace prueba.api.Application.Alquiler.CrearAlquiler
{
   public class CrearAlquilerCommandHandler : ICommandHandler<CrearAlquilerCommand, Guid>
    {

        private readonly ILogger<CrearAlquilerCommandHandler> _logger;

        public CrearAlquilerCommandHandler(ILogger<CrearAlquilerCommandHandler> logger)
        {
            _logger = logger;
        }

        public Task<Guid> Handle(CrearAlquilerCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creando alquiler para el cliente {IdCliente} y el vehículo {IdVehiculo}", command.IdCliente, command.IdVehiculo);
              return Task.FromResult(Guid.NewGuid());
        }

        ValueTask<Guid> ICommandHandler<CrearAlquilerCommand, Guid>.Handle(CrearAlquilerCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creando alquiler para el cliente {IdCliente} y el vehículo {IdVehiculo}", command.IdCliente, command.IdVehiculo);
              return ValueTask.FromResult(Guid.NewGuid());
        }
    }
    
}