using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mediator;



namespace prueba.api.Application.Alquiler.CrearAlquiler;

public record CrearAlquilerCommand(int IdCliente, int IdVehiculo) : ICommand<Guid>;
   
