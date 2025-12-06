using Arquetipo.Application.Shared.Users;
using Arquetipo.Domain.Abstractions;
using Arquetipo.Domain.Users;
using ErrorOr;
using Mediator;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arquetipo.Application.Users.CreateUser
{
    internal sealed class CreateUserCommandHandler(IUserRepository _userRepository, IUnitOfWork _unitOfWork, ILogger<CreateUserCommandHandler> _logger) 
    : ICommandHandler<CreateUserCommand, ErrorOr<UserResponse>>
    {



        public async ValueTask<ErrorOr<UserResponse>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {

            bool userExists = await _userRepository.IsUserExists(command.Name);

            if (userExists)
            {
                _logger.LogInformation("User name {UserName} already exists", command.Name);
 
                return Error.Conflict($"User Name {command.Name} already exists");
            }
            _logger.LogInformation("Creando el usuario {Id} y con nombre {Name}", command.Id, command.Name);
            User user = User.Create(command.Id, command.Name);
            _userRepository.Add(user);
            await _unitOfWork.SaveChangesAsync();

            var response = new UserResponse(user.Id, user.Name!, user.Guid);

            return response;
        }
    }
}
