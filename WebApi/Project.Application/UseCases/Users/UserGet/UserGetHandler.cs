using Common.CleanArch;
using Microsoft.Extensions.Logging;
using Project.Application.Dtos.Users;
using Project.Application.UseCases.Users.UserGet.Responses;
using Project.Domain.IRepositories;

namespace Project.Application.UseCases.Users.UserGet
{
    internal sealed class UserGetHandler : IInteractor<UserGetRequest, UserGetResponse>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ILogger<UserGetHandler> _logger;

        public UserGetHandler(IUsersRepository usersRepository, ILogger<UserGetHandler> logger)
        {
            _usersRepository = usersRepository;
            _logger = logger;
        }

        public async Task<UserGetResponse> Handle(UserGetRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Consultando usuario {UserId}", request.UserId);

            var user = await _usersRepository
                .GetByIdAsync(request.UserId, cancellationToken)
                .ConfigureAwait(false);

            if (user is null)
            {
                _logger.LogWarning("No se encontro usuario {UserId}", request.UserId);
                return new FailureUserGetResponse("El usuario no existe.");
            }

            var dto = Map(user);
            _logger.LogInformation("Usuario {UserId} consultado exitosamente", request.UserId);
            return new SuccessUserGetResponse(dto);
        }

        private static UserDataDto Map(Project.Domain.Entities.User user) =>
            new(
                user.UserId,
                user.UserFullName,
                user.UserName,
                user.RoleId,
                user.UserActive,
                user.UtcTimeStamp);
    }
}
