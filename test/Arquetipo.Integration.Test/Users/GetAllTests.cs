using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arquetipo.Application.Users.GetAll;
using Arquetipo.Domain.Users;
using Arquetipo.Integration.Tests.Infrastructure;

namespace Arquetipo.Integration.Test.Users;

public class GetAllTests : BaseIntegrationTest
{
    public GetAllTests(IntegrationTestWebAppFactory factory) : base(factory)
    {

    }
    [Fact]
    public async Task GetAllUsers_ShouldReturnAllUsers_WhenUsersExist()
    {
        // Arrange: limpiar cualquier dato previo y preparar datos nuevos
        await ResetDatabaseAsync();

        // Act
        var query = new GetAllUsersQuery();
        var result = await Sender.Send(query);

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(2, result.Value.Count);

        Assert.Contains(result.Value, u => u.Name == "Manolo");
        Assert.Contains(result.Value, u => u.Name == "Alberto");
    }
    [Fact]
    public async Task GetAllUsers_ShouldMapFieldsCorrectly()
    {
        // Arrange: limpiar y preparar solo un usuario
        dbContext.Users.RemoveRange(dbContext.Users);
        await dbContext.SaveChangesAsync();

        var user = User.Create(10, "Carlos");
        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();

        // Act
        var query = new GetAllUsersQuery();
        var result = await Sender.Send(query);

        // Assert
        var response = result.Value.Single();

        Assert.Equal(user.Id, response.Id);
        Assert.Equal(user.Name, response.Name);

    }
    [Fact]
    public async Task GetAllUsers_ShouldReturnEmptyList_WhenNoUsersExist()
    {
        // Arrange: limpiar la base completamente
        dbContext.Users.RemoveRange(dbContext.Users);
        await dbContext.SaveChangesAsync();

        // Act
        var query = new GetAllUsersQuery();
        var result = await Sender.Send(query);

        // Assert
        Assert.False(result.IsError);
        Assert.Empty(result.Value);
    }



}
