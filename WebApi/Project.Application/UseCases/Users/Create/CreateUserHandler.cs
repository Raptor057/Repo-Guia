using Common.CleanArch;
using Microsoft.Extensions.Logging;
using Project.Application.UseCases.Users.Common;
using Project.Domain.Entities;
using Project.Domain.IRepositories;

namespace Project.Application.UseCases.Users.Create
{
    public sealed class CreateUserHandler : IInteractor<CreateUserRequest, UserCommandResponse>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ILogger<CreateUserHandler> _logger;

        public CreateUserHandler(IUsersRepository usersRepository, ILogger<CreateUserHandler> logger)
        {
            _usersRepository = usersRepository;
            _logger = logger;
        }

        public async Task<UserCommandResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken)
        {
            if (await _usersRepository.UsernameExistsAsync(request.UserName, cancellationToken).ConfigureAwait(false))
            {
                _logger.LogWarning("No se puede crear el usuario {User} porque ya existe", request.UserName);
                return new FailureUserCommandResponse("El usuario ya existe.", 409);
            }

            var user = new User
            {
                UserName = request.UserName,
                PasswordHash = request.Password,
                FullName = request.FullName,
                UserActive = true
            };

            var newId = await _usersRepository.CreateUserAsync(user, cancellationToken).ConfigureAwait(false);
            if (newId <= 0)
            {
                _logger.LogError("No se pudo crear el usuario {User}", request.UserName);
                return new FailureUserCommandResponse("No se pudo crear el usuario.");
            }

            _logger.LogInformation("Usuario {User} creado con id {Id}", request.UserName, newId);
            return new SuccessUserCommandResponse(newId, user.UserName, user.FullName, user.UserActive, "Usuario creado correctamente.");
        }
    }
}
