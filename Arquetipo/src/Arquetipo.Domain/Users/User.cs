using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arquetipo.Domain.Abstractions;

namespace Arquetipo.Domain.Users;

public sealed class User : Entity<Guid>
{
    public int UserId {get; private set;}
    public string? Name {get; private set;}
    
    private User(){}
    
    private User(Guid id, int userId, string name) : base(id)
    {
        UserId = userId;
        Name = name;
    }

    public static User Create(int userId, string name)
    {
        var user = new User(Guid.NewGuid(), userId, name);
        return user;
    }
}
