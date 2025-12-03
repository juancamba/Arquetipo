using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arquetipo.Application.Abstractions.Email;
using Arquetipo.Domain.Users.Events;
using Mediator;

namespace Arquetipo.Application.Users.CreateUser;

internal sealed class CreateUserDomainEventHandler : INotificationHandler<UserCreatedDomainEvent>
{

    private readonly IEmailService _emailService;

    public CreateUserDomainEventHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async ValueTask Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // si tienes que recuperar información de base de datos como el nombre del usuario o lo que sea es aqui.
        // inyecta un repository, un service o lo que haga falta parar recuperar la informaicon del evento
        
        Console.WriteLine("Enviar evento usuario por email");
         await _emailService.SendAsync(
            "arquetipo@arquetipo.com",
            "arquetipo@arquetipo.com",
            "User Created",
            $"User with id {notification.UserId}"
        );
    }
}
