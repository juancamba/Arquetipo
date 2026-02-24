using Arquetipo.Domain.Abstractions;

namespace Arquetipo.Domain.UnitTests.Infrastructure;

public abstract class BaseTest
{

    public static T AssertDomainEventWasPublished<T>(IEntity entity)
    where T : IDomainEvent
    {
        var domainEvent = entity.GetDomainEvents().OfType<T>().SingleOrDefault();
        if (domainEvent is null)
        {
            throw new Exception($"{typeof(T).Name} was not published");
        }
        return domainEvent!;
    }

    protected static void AssertNoDomainEvents(IEntity entity)
    {
        var any = entity.GetDomainEvents().Any();

        if (any)
            throw new Exception("No published domain events were expected");
    }
}