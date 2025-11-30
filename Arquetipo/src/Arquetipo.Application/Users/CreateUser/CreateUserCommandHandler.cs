using Mediator;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arquetipo.Application.Users.CreateUser
{
    internal class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, int>
    {

        private readonly ILogger<CreateUserCommandHandler> _logger;

        public CreateUserCommandHandler(ILogger<CreateUserCommandHandler> logger)
        {
            _logger = logger;
        }

        public ValueTask<int> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creando el usuario {Id} y con nombre {Name}", command.Id, command.Name);
            return ValueTask.FromResult(command.Id);
        }
    }
}
