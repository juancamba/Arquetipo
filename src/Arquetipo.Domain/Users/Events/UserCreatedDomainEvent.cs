using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arquetipo.Domain.Abstractions;

namespace Arquetipo.Domain.Users.Events;

public sealed record UserCreatedDomainEvent(int UserId) : IDomainEvent;

