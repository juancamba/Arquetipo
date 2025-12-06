using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arquetipo.Domain.Users;

namespace Arquetipo.Application.UnitTest.Users;

internal static class UserMock
{
    public static User Create() => User.Create(Id, Guid, Name );

    public static readonly Guid Guid= new Guid("");
    public static readonly string Name = "Lio Messi";
    public static int Id  = 1;
}
