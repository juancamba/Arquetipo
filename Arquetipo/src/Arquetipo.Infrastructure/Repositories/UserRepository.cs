using Arquetipo.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arquetipo.Infrastructure.Repositories;

internal sealed class UserRepository : Repository<User, int>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        
    }

    public Task<bool> IsUserExists(string name, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
