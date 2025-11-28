using Common.CleanArch;
using Microsoft.Extensions.Logging;
using Project.Application.UseCases.Users.UserCreate.Responses;
using Project.Domain.IRepositories;

namespace Project.Application.UseCases.Users.UserCreate
{
    internal sealed class UserCreateHandler : IInteractor<UserCreateRequest, UserCreateResponse>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ILogger<UserCreateHandler> _logger;

        public UserCreateHandler(IUsersRepository usersRepository, ILogger<UserCreateHandler> logger)
        {
            _usersRepository = usersRepository;
            _logger = logger;
        }

        public async Task<UserCreateResponse> Handle(UserCreateRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creando usuario {UserName}", request.UserName);

            var exists = await _usersRepository
                .UserNameExistsAsync(request.UserName, cancellationToken)
                .ConfigureAwait(false);

            if (exists)
            {
                _logger.LogWarning("No se pudo crear usuario {UserName} porque ya existe", request.UserName);
                return new FailureUserCreateResponse($"El usuario \"{request.UserName}\" ya existe.");
            }

            var userId = await _usersRepository
                .CreateUserAsync(request.FullName, request.UserName, request.PasswordHash, request.RoleId, cancellationToken)
                .ConfigureAwait(false);

            if (userId <= 0)
            {
                _logger.LogError("Fallo al insertar usuario {UserName} en la base de datos", request.UserName);
                return new FailureUserCreateResponse("Ocurrió un problema al crear el usuario.");
            }

            _logger.LogInformation("Usuario {UserName} creado con ID {UserId}", request.UserName, userId);
            return new SuccessUserCreateResponse(userId, "Usuario creado exitosamente.");
        }
    }
}
