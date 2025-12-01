using Arquetipo.Domain.Abstractions;
using Arquetipo.Domain.Users;
using Microsoft.EntityFrameworkCore;
namespace Arquetipo.Infrastructure;

public sealed class ApplicationDbContext : DbContext, IUnitOfWork
{
     public DbSet<User> Users { get; set; }
     
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)  {  }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {/*
        try
        {
            
            
            //AddDomainEventsToOutboxMessages();
            var result = await base.SaveChangesAsync(cancellationToken);
            //  await PublishDomainEventsAsync();
            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException("La excepcion por concurrencia se disparo", ex);
        }
        */

        return await base.SaveChangesAsync(cancellationToken);
    }

    private void AddDomainEventsToOutboxMessages()
    {
        /*
        var outboxMessages = ChangeTracker
            .Entries<IEntity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                var domainEvents = entity.GetDomainEvents();
                entity.ClearDomainEvents();
                return domainEvents;
            }).Select(domainEvent=> new OutboxMessage(
                Guid.NewGuid(), 
                _dateTimeProvider.currentTime,
                domainEvent.GetType().Name,
                JsonConvert.SerializeObject(domainEvent,jsonSerializerSettings)
            ))
            .ToList()
            
            ;
        AddRange(outboxMessages);
        */

    }

    private async Task PublishDomainEventsAsync()
    {
        var domainEvents = ChangeTracker
            .Entries<IEntity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity => 
            {
                var domainEvents = entity.GetDomainEvents();
                entity.ClearDomainEvents();
                return domainEvents;
            }).ToList();
        
        foreach(var domainEvent in domainEvents)
        {
            //await _publisher.Publish(domainEvent);
        }

    }


}