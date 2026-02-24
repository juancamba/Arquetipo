using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arquetipo.Application.Shared.Users;
using Arquetipo.Application.Users.CreateUser;
using Arquetipo.Domain.Abstractions;
using Arquetipo.Domain.Users;
using ErrorOr;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Moq;

namespace Arquetipo.Application.UnitTest.Users
{

    public class CreateUserTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<ILogger<CreateUserCommandHandler>> _loggerMock = new();
        private readonly Mock<IMapper> _mapperMock = new();

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
                _loggerMock.Object,
                _mapperMock.Object
            );

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsError);
            Assert.Equal(ErrorType.Conflict, result.FirstError.Type);

            _userRepositoryMock.Verify(r => r.Add(It.IsAny<User>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);

            // El mapper no debe ejecutarse en conflicto
            _mapperMock.Verify(m => m.Map<UserResponse>(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldCreateUser_WhenUserDoesNotExist()
        {
            // Arrange
            var command = new CreateUserCommand(1, "Carlos");
            var someGuid = Guid.NewGuid();
            var expected = new UserResponse(1, "Carlos", someGuid);

            _userRepositoryMock
                .Setup(repo => repo.IsUserExists("Carlos"))
                .ReturnsAsync(false);

            _mapperMock
                .Setup(m => m.Map<UserResponse>(It.IsAny<User>()))
                .Returns(expected);

            var handler = new CreateUserCommandHandler(
                _userRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _loggerMock.Object,
                _mapperMock.Object
            );

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsError);
            Assert.Equal(expected, result.Value);

            _userRepositoryMock.Verify(r => r.Add(It.IsAny<User>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
            _mapperMock.Verify(m => m.Map<UserResponse>(It.IsAny<User>()), Times.Once);
        }

    }
}