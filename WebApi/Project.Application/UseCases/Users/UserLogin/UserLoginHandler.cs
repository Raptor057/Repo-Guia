using Common.CleanArch;
using Microsoft.Extensions.Logging;
using Project.Application.UseCases.Users.UserLogin.Responses;
using Project.Domain.IRepositories;

namespace Project.Application.UseCases.Users.UserLogin
{
    internal sealed class UserLoginHandler : IInteractor<UserLoginRequest, UserLoginResponse>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ILogger<UserLoginHandler> _logger;

        public UserLoginHandler(IUsersRepository usersRepository, ILogger<UserLoginHandler> logger)
        {
            _usersRepository = usersRepository;
            _logger = logger;
        }

        public async Task<UserLoginResponse> Handle(UserLoginRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Intento de login para usuario {User}", request.User);

            var user = await _usersRepository
                .GetUserForLoginAsync(request.User, request.Password, cancellationToken)
                .ConfigureAwait(false);

            if (user is null)
            {
                _logger.LogWarning("Login fallido para usuario {User}", request.User);
                return new FailureUserLoginResponse("Usuario o contraseña incorrectos.");
            }

            _logger.LogInformation("Login exitoso para usuario {User}", request.User);
            return new SuccessUserLoginResponse(
                user.UserId,
                user.UserName,
                user.UserFullName,
                user.RoleId,
                user.RoleName,
                "Login exitoso.");
        }
    }
}
