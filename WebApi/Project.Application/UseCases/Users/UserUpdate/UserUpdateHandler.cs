using Common.CleanArch;
using Microsoft.Extensions.Logging;
using Project.Application.UseCases.Users.UserUpdate.Responses;
using Project.Domain.IRepositories;

namespace Project.Application.UseCases.Users.UserUpdate
{
    internal sealed class UserUpdateHandler : IInteractor<UserUpdateRequest, UserUpdateResponse>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ILogger<UserUpdateHandler> _logger;

        public UserUpdateHandler(IUsersRepository usersRepository, ILogger<UserUpdateHandler> logger)
        {
            _usersRepository = usersRepository;
            _logger = logger;
        }

        public async Task<UserUpdateResponse> Handle(UserUpdateRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Actualizando usuario {UserId}", request.UserId);

            var exists = await _usersRepository
                .UserExistsAsync(request.UserId, cancellationToken)
                .ConfigureAwait(false);

            if (!exists)
            {
                _logger.LogWarning("No se encontro usuario {UserId} para actualizar", request.UserId);
                return new FailureUserUpdateResponse("El usuario no existe.");
            }

            var userNameExists = await _usersRepository
                .UserNameExistsAsync(request.UserName, request.UserId, cancellationToken)
                .ConfigureAwait(false);

            if (userNameExists)
            {
                _logger.LogWarning("No se pudo actualizar usuario {UserId} porque el nombre {UserName} ya existe", request.UserId, request.UserName);
                return new FailureUserUpdateResponse($"El usuario \"{request.UserName}\" ya existe.");
            }

            var updated = await _usersRepository
                .UpdateUserAsync(request.UserId, request.FullName, request.UserName, request.PasswordHash, request.RoleId, cancellationToken)
                .ConfigureAwait(false);

            if (!updated)
            {
                _logger.LogError("Fallo al actualizar usuario {UserId}", request.UserId);
                return new FailureUserUpdateResponse("No se pudo actualizar el usuario.");
            }

            _logger.LogInformation("Usuario {UserId} actualizado exitosamente", request.UserId);
            return new SuccessUserUpdateResponse("Usuario actualizado exitosamente.");
        }
    }
}
