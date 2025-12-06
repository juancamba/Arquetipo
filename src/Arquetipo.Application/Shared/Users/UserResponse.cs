using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arquetipo.Application.Shared.Users;
public record UserResponse(int Id, string Name, Guid guid);