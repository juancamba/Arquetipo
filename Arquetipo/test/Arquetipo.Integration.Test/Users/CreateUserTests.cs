

using Arquetipo.Application.Users.CreateUser;
using Arquetipo.Domain.Users;
using Arquetipo.Integration.Tests.Infrastructure;
using ErrorOr;
using Mediator;
using Microsoft.EntityFrameworkCore;
namespace Arquetipo.Integration.Test.Users;

public class CreateUserTests : BaseIntegrationTest
{

    public CreateUserTests(IntegrationTestWebAppFactory factory) : base(factory)
    {

    }

    [Fact]
    public async Task CreateUser_ShouldReturnOk_WhenUserDoesntExists()
    {
await ResetDatabaseAsync();

        var commnad = new CreateUserCommand(3, "Tom");
        var result = await Sender.Send(commnad);

        // Assert
        Assert.False(result.IsError);
        Assert.Equal("Tom", result.Value.Name);
        Assert.Equal(3, result.Value.Id);

        // comprobamos ambas cosas (respuesta del handler y lo que hay en db), porque el handler puede tener errores lógicos

        // Verify DB persisted properly
        var userInDb = await dbContext.Users.FirstOrDefaultAsync(u => u.Name == "Tom");

        Assert.NotNull(userInDb);
        Assert.Equal(result.Value.Id, userInDb.Id);



    }

    [Fact]
    public async Task CreateUser_ShouldReturnConflict_WhenUserAlreadyExists()
    {
        await ResetDatabaseAsync();


        var command = new CreateUserCommand(11, "Manolo");

        // Act
        var result = await Sender.Send(command);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(ErrorType.Conflict, result.FirstError.Type);

        // DB should still contain only the original user
        var users = await dbContext.Users.Where(u => u.Name == "Manolo").ToListAsync();
        Assert.Single(users); // no se crea un duplicado
    }

    [Fact]
    public async Task CreateUser_ShouldNotPersistUser_WhenConflictOccurs()
    {
        await ResetDatabaseAsync();


        var command = new CreateUserCommand(21, "Manolo");

        // Act
        var result = await Sender.Send(command);

        // Assert: confirm conflict
        Assert.True(result.IsError);

        // Assert: confirm database unchanged
        var count = await dbContext.Users.CountAsync(u => u.Name == "Manolo");
        Assert.Equal(1, count);
    }


}
