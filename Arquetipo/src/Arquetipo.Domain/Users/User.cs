using Arquetipo.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Arquetipo.Domain.Users;

public sealed class User : Entity<int>
{
    public Guid Guid { get; private set; }
    public string? Name { get; private set; }

    private User() { }

    private User(int id, Guid guid, string name) : base(id)
    {
        Guid = guid;
        Name = name;
    }

    public static User Create(int id, string name)
    {
        var user = new User(id, Guid.NewGuid(), name);
        return user;
    }

    public static User Create(int id, Guid guid, string name) => new(id, guid, name);

}
