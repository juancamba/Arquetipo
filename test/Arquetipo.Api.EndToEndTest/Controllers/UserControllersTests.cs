using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arquetipo.Api.Controllers;
using Arquetipo.Api.Dto.Users;
using Arquetipo.Application.Shared.Users;
using Arquetipo.Application.Users.CreateUser;
using Arquetipo.Application.Users.GetAll;
using ErrorOr;
using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;


namespace Arquetipo.Api.EndToEndTest.Controllers;

public class UserControllersTests
{
    private readonly UsersController _controller;
    private readonly Mock<IMediator> _mediatorMock;

    public UserControllersTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new UsersController(_mediatorMock.Object);
    }
    [Fact]
    public async Task Create_ShouldReturnOk_WhenMediatorReturnsSuccess()
    {
        // Arrange
        var userDto = new UserDto(1, "Tom");
        var response = new UserResponse(1, "Tom", Guid.NewGuid());

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response); // Simula respuesta exitosa

        // Act
        var result = await _controller.Create(userDto, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<Ok<UserResponse>>(result);
        var value = Assert.IsType<UserResponse>(okResult.Value);
        Assert.Equal("Tom", value.Name);
        Assert.Equal(1, value.Id);
    }
    [Fact]
    public async Task Create_ShouldReturnProblem_WhenMediatorReturnsError()
    {
        // Arrange
        var userDto = new UserDto(1, "Tom");
        var error = Error.Conflict("User already exists");

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(error); // Simula error

        // Act
        var result = await _controller.Create(userDto, CancellationToken.None);


        // Assert
        var problemResult = Assert.IsAssignableFrom<IResult>(result);
        dynamic dynamicResult = problemResult;
        int? statusCode = dynamicResult?.StatusCode;

        Assert.Equal(StatusCodes.Status409Conflict, statusCode);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk_WithUsersList()
    {
        // Arrange
        var users = new List<UserResponse>
            {
                new UserResponse(1, "Tom", Guid.NewGuid()),
                new UserResponse(2, "Manolo", Guid.NewGuid())
            };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllUsersQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);

        // Act
        var result = await _controller.GetAll(CancellationToken.None);

        // Assert
        var okResult = Assert.IsAssignableFrom<IResult>(result);
        if (okResult is Ok<List<UserResponse>> okTyped)
        {
            Assert.Equal(2, okTyped.Value!.Count);
            Assert.Contains(okTyped.Value, u => u.Name == "Tom");
            Assert.Contains(okTyped.Value, u => u.Name == "Manolo");
        }
        else
        {
            throw new Xunit.Sdk.XunitException("Expected Ok<UserResponse>");
        }
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk_WithEmptyList_WhenNoUsersExist()
    {
        // Arrange
        var emptyUsers = new List<UserResponse>();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllUsersQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(emptyUsers);

        // Act
        var result = await _controller.GetAll(CancellationToken.None);

        // Assert
        var okResult = Assert.IsAssignableFrom<IResult>(result);
        if (okResult is Ok<List<UserResponse>> okTyped)
        {
            Assert.Empty(okTyped.Value!); // La lista debe estar vacía
        }
        else
        {
            throw new Xunit.Sdk.XunitException("Expected Ok<List<UserResponse>>");
        }
    }


}
