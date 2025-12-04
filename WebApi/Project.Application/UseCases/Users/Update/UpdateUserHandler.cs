using Common.CleanArch;
using Microsoft.Extensions.Logging;
using Project.Application.UseCases.Users.Common;
using Project.Domain.IRepositories;

namespace Project.Application.UseCases.Users.Update
{
    public sealed class UpdateUserHandler : IInteractor<UpdateUserRequest, UserCommandResponse>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ILogger<UpdateUserHandler> _logger;

        public UpdateUserHandler(IUsersRepository usersRepository, ILogger<UpdateUserHandler> logger)
        {
            _usersRepository = usersRepository;
            _logger = logger;
        }

        public async Task<UserCommandResponse> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
        {
            var existing = await _usersRepository.GetByIdAsync(request.UserId, cancellationToken).ConfigureAwait(false);
            if (existing is null)
            {
                _logger.LogWarning("Usuario {Id} no encontrado para actualizar", request.UserId);
                return new FailureUserCommandResponse("Usuario no encontrado.", 404);
            }

            var updated = await _usersRepository
                .UpdateUserAsync(request.UserId, request.FullName, request.Password, cancellationToken)
                .ConfigureAwait(false);

            if (!updated)
            {
                _logger.LogWarning("No se pudo actualizar el usuario {Id}", request.UserId);
                return new FailureUserCommandResponse("No se pudo actualizar el usuario.");
            }

            var fullName = request.FullName ?? existing.FullName;
            var active = existing.UserActive;
            _logger.LogInformation("Usuario {Id} actualizado", request.UserId);
            return new SuccessUserCommandResponse(request.UserId, existing.UserName, fullName, active, "Usuario actualizado correctamente.");
        }
    }
}
