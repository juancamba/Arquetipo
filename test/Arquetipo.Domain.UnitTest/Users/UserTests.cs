
using Arquetipo.Domain.UnitTests.Infrastructure;
using Arquetipo.Domain.Users;
using Arquetipo.Domain.Users.Events;

using Xunit;

namespace Arquetipo.Domain.UnitTests.Users;

public class UserTests : BaseTest
{

    [Fact]
    public void Create_Should_SetPropertyValues()
    {

        //Arrange --> Vamos a crear un Mock File-> UserMock

        // Act
        var user = User.Create(
            UserMock.Id, 
            UserMock.Name
         
            );

        // Assert 
        Assert.Equal(UserMock.Id,   user.Id);
        Assert.Equal(UserMock.Name, user.Name);
        Assert.NotEqual(Guid.Empty, user.Guid);
     
    }

    [Fact]
    public void Create_Should_RaiseUserCreateDomainEvent()
    {
         var user = User.Create(
            UserMock.Id, 
            UserMock.Name
         
            );

        //representacion abstracta
        var domainEvent = AssertDomainEventWasPublished<UserCreatedDomainEvent>(user);


        Assert.NotNull(domainEvent);

        // Validamos que UserId sea igual
        Assert.Equal(user.Id, domainEvent.UserId);
    }


  


}