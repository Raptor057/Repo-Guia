using Common.CleanArch;
using Microsoft.Extensions.Logging;
using Project.Application.UseCases.Users.UserDisable.Responses;
using Project.Domain.IRepositories;

namespace Project.Application.UseCases.Users.UserDisable
{
    internal sealed class UserDisableHandler : IInteractor<UserDisableRequest, UserDisableResponse>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ILogger<UserDisableHandler> _logger;

        public UserDisableHandler(IUsersRepository usersRepository, ILogger<UserDisableHandler> logger)
        {
            _usersRepository = usersRepository;
            _logger = logger;
        }

        public async Task<UserDisableResponse> Handle(UserDisableRequest request, CancellationToken cancellationToken)
        {
            var action = request.IsActive ? "Activando" : "Deshabilitando";
            _logger.LogInformation("{Action} usuario {UserId}", action, request.UserId);

            var exists = await _usersRepository
                .UserExistsAsync(request.UserId, cancellationToken)
                .ConfigureAwait(false);

            if (!exists)
            {
                _logger.LogWarning("No se encontro usuario {UserId} para actualizar su estado", request.UserId);
                return new FailureUserDisableResponse("El usuario no existe.");
            }

            var updated = await _usersRepository
                .UpdateUserActiveAsync(request.UserId, request.IsActive, cancellationToken)
                .ConfigureAwait(false);

            if (!updated)
            {
                _logger.LogError("Fallo al actualizar estado de usuario {UserId}", request.UserId);
                return new FailureUserDisableResponse("No se pudo actualizar el estado del usuario.");
            }

            var successMessage = request.IsActive ? "Usuario activado exitosamente." : "Usuario deshabilitado exitosamente.";
            _logger.LogInformation("Usuario {UserId} actualizado a {Active}", request.UserId, request.IsActive);
            return new SuccessUserDisableResponse(successMessage);
        }
    }
}
