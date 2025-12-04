using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arquetipo.Application.Users.CreateUser;
using Arquetipo.Domain.Abstractions;
using Arquetipo.Domain.Users;
using ErrorOr;
using Microsoft.Extensions.Logging;
using Moq;

namespace Arquetipo.Application.UnitTest.Users
{

    public class UserCreateTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<ILogger<CreateUserCommandHandler>> _loggerMock = new();



        [Fact]
        public async Task Handle_ShouldReturnConflict_WhenUserAlreadyExists()
        {
            // Arrange
            var command = new CreateUserCommand(1, "Juan");

            _userRepositoryMock
                .Setup(repo => repo.IsUserExists("Juan"))
                .ReturnsAsync(true);

            var handler = new CreateUserCommandHandler(
                _userRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _loggerMock.Object
            );

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsError);
            Assert.Equal(ErrorType.Conflict, result.FirstError.Type);

            _userRepositoryMock.Verify(r => r.Add(It.IsAny<User>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);


        }

        [Fact]
        public async Task Handle_ShouldCreateUser_WhenUserDoesNotExist()
        {
            // Arrange
            var command = new CreateUserCommand(1, "Carlos");

            _userRepositoryMock
                .Setup(repo => repo.IsUserExists("Carlos"))
                .ReturnsAsync(false);

            var handler = new CreateUserCommandHandler(
                _userRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _loggerMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsError);
        }
    }
}