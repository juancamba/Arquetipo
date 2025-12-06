using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arquetipo.Domain.Abstractions;
namespace Arquetipo.Domain.Users;

public interface IUserRepository : IRepository<User, int>
{

   // Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default);

   // void Add(User user);
    

    //Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken= default);

    Task<bool> IsUserExists(string name, CancellationToken cancellationToken = default);

}
