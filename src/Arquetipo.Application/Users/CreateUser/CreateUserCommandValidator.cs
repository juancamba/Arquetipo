using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arquetipo.Application.Users.CreateUser
{
    internal sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {

            RuleFor(c => c.Id).NotEmpty().WithMessage("El id es obligatorio");
            RuleFor(c => c.Name).NotEmpty().WithMessage("El Name es obligatorio");
        }
    }
}
