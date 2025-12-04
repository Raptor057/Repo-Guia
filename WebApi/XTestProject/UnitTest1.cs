using Moq;
using Project.Application.Dtos.Users;
using Project.Application.UseCases.Users.Create;
using Project.Application.UseCases.Users.Disable;
using Project.Application.UseCases.Users.Update;
using Project.Application.UseCases.Users.UserLogin;
using Project.Application.UseCases.Users.UserLogin.Responses;
using Project.Domain.Entities;
using Project.Domain.IRepositories;

namespace XTestProject
{
    public class UsersUseCasesTests
    {
        [Fact]
        public async Task CreateUserHandler_Should_ReturnConflict_WhenUserExists()
        {
            var repo = new Mock<IUsersRepository>();
            repo.Setup(r => r.UsernameExistsAsync("taken", It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var handler = new CreateUserHandler(repo.Object, Mock.Of<Microsoft.Extensions.Logging.ILogger<CreateUserHandler>>());
            var request = CreateUserRequest.Create(new CreateUserDto("taken", "pass", "Tester"));

            var response = await handler.Handle(request, CancellationToken.None);

            Assert.IsType<Project.Application.UseCases.Users.Common.FailureUserCommandResponse>(response);
        }

        [Fact]
        public async Task CreateUserHandler_Should_HashPassword_And_ReturnSuccess()
        {
            var repo = new Mock<IUsersRepository>();
            User? savedUser = null;
            repo.Setup(r => r.UsernameExistsAsync("new", It.IsAny<CancellationToken>())).ReturnsAsync(false);
            repo.Setup(r => r.CreateUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Callback<User, CancellationToken>((u, _) => savedUser = u)
                .ReturnsAsync(5);

            var handler = new CreateUserHandler(repo.Object, Mock.Of<Microsoft.Extensions.Logging.ILogger<CreateUserHandler>>());
            var request = CreateUserRequest.Create(new CreateUserDto("new", "MyPassword123", "Tester"));

            var response = await handler.Handle(request, CancellationToken.None);

            Assert.True(savedUser?.PasswordHash?.Length == 64);
            Assert.IsType<Project.Application.UseCases.Users.Common.SuccessUserCommandResponse>(response);
        }

        [Fact]
        public async Task UpdateUserHandler_Should_ReturnNotFound_WhenMissing()
        {
            var repo = new Mock<IUsersRepository>();
            repo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);

            var handler = new UpdateUserHandler(repo.Object, Mock.Of<Microsoft.Extensions.Logging.ILogger<UpdateUserHandler>>());
            var request = UpdateUserRequest.Create(new UpdateUserDto(1, "pass", "Name"));

            var response = await handler.Handle(request, CancellationToken.None);

            var failure = Assert.IsType<Project.Application.UseCases.Users.Common.FailureUserCommandResponse>(response);
            Assert.Equal(404, failure.StatusCode);
        }

        [Fact]
        public async Task DisableUserHandler_Should_ReturnSuccess_WhenAlreadyDisabled()
        {
            var repo = new Mock<IUsersRepository>();
            repo.Setup(r => r.GetByIdAsync(2, It.IsAny<CancellationToken>())).ReturnsAsync(new User
            {
                Id = 2,
                UserName = "disabled",
                FullName = "User Disabled",
                PasswordHash = "HASH",
                UserActive = false
            });

            var handler = new DisableUserHandler(repo.Object, Mock.Of<Microsoft.Extensions.Logging.ILogger<DisableUserHandler>>());
            var request = DisableUserRequest.Create(2);

            var response = await handler.Handle(request, CancellationToken.None);

            Assert.IsType<Project.Application.UseCases.Users.Common.SuccessUserCommandResponse>(response);
        }

        [Fact]
        public void UserLoginRequest_Should_ValidateFields()
        {
            var dto = new UserLoginDto(" ", " ");
            Assert.Throws<InvalidOperationException>(() => UserLoginRequest.Login(dto));
        }
    }
}
