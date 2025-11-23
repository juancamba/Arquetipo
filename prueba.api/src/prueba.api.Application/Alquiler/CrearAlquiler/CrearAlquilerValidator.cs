using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace prueba.api.Application.Alquiler.CrearAlquiler
{
    public class CrearAlquilerValidator : AbstractValidator<CrearAlquilerCommand>
    {
        public CrearAlquilerValidator()
        {
            RuleFor(x => x.IdCliente)
                .GreaterThan(0).WithMessage("El IdCliente debe ser mayor que cero.");

            RuleFor(x => x.IdVehiculo)
                .GreaterThan(0).WithMessage("El IdVehiculo debe ser mayor que cero.");
        }
    }
}