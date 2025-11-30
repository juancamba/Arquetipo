using Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arquetipo.Application.Users.CreateUser;

public sealed record CreateUserCommand(int Id, string Name) : ICommand<int>;
