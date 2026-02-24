using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arquetipo.Application.Users.CreateUser
{
    public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {

            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("El id es obligatorio")
                .NotNull().WithMessage("El id no puede ser null")
                .GreaterThan(0).WithMessage("El id debe ser > 0");
                ;
            RuleFor(c => c.Name).NotEmpty().WithMessage("El Name es obligatorio");
        }
    }
}
