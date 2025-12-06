using Arquetipo.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arquetipo.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
internal sealed class UserRepository : Repository<User, int>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
    {

    }

    public async Task<bool> IsUserExists(string name, CancellationToken cancellationToken = default)
    {
        return await _appDbContext.Users.AnyAsync(c => c.Name == name, cancellationToken);

    }
}
