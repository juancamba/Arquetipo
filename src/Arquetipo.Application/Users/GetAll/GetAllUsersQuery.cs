using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arquetipo.Application.Shared.Users;
using ErrorOr;
using Mediator;

namespace Arquetipo.Application.Users.GetAll;

public sealed record GetAllUsersQuery() : IQuery<ErrorOr<List<UserResponse>>>;