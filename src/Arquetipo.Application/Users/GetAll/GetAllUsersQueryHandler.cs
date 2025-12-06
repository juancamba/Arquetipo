using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arquetipo.Application.Shared.Users;
using Arquetipo.Domain.Users;
using ErrorOr;
using Mediator;
using Microsoft.Extensions.Logging;

namespace Arquetipo.Application.Users.GetAll;

internal sealed class GetAllUsersQueryHandler(IUserRepository _userRepository)
    : IQueryHandler<GetAllUsersQuery, ErrorOr<List<UserResponse>>>
{
    public async ValueTask<ErrorOr<List<UserResponse>>> Handle(
        GetAllUsersQuery query,
        CancellationToken cancellationToken)
    {


        var users = await _userRepository.GetAllAsync(cancellationToken);

        var response = users
            .Select(u => new UserResponse(u.Id, u.Name!, u.Guid))
            .ToList();

        return response;
    }
}

