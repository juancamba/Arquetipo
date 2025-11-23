using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arquetipo.Domain.Abstractions;

    public interface IEntity
    {
        IReadOnlyList<IDomainEvent> GetDomainEvents();
        void ClearDomainEvents();
    }
