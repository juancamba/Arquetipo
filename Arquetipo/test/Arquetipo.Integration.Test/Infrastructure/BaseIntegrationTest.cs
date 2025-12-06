using Arquetipo.Domain.Users;
using Arquetipo.Infrastructure;
using Mediator;

using Microsoft.Extensions.DependencyInjection;

namespace Arquetipo.Integration.Tests.Infrastructure;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
{
    private readonly IServiceScope _scope;
    protected readonly ISender Sender;
    protected readonly ApplicationDbContext dbContext;

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();
        Sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        dbContext = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        /*
        User user1 = User.Create(1, "Manolo");
        User user2 = User.Create(2, "Alberto");

        if (!dbContext.Users.Any())
        {
            dbContext.Users.Add(user2);
            dbContext.Users.Add(user1);
            dbContext.SaveChanges();
        }
        */
    }
    public async Task ResetDatabaseAsync()
    {
        dbContext.Users.RemoveRange(dbContext.Users);
        await dbContext.SaveChangesAsync();
        User user1 = User.Create(1, "Manolo");
        User user2 = User.Create(2, "Alberto");
        await dbContext.Users.AddAsync(user2);
        await dbContext.Users.AddAsync(user1);
        await dbContext.SaveChangesAsync();
    }

}