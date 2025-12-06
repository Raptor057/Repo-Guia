using Common.CleanArch;
using Microsoft.Extensions.Logging;
using Project.Application.Dtos.Users;
using Project.Application.UseCases.Users.UserList.Responses;
using Project.Domain.IRepositories;

namespace Project.Application.UseCases.Users.UserList
{
    internal sealed class UserListHandler : IInteractor<UserListRequest, UserListResponse>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ILogger<UserListHandler> _logger;

        public UserListHandler(IUsersRepository usersRepository, ILogger<UserListHandler> logger)
        {
            _usersRepository = usersRepository;
            _logger = logger;
        }

        public async Task<UserListResponse> Handle(UserListRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Consultando lista de usuarios");

            var users = await _usersRepository
                .GetListAsync(cancellationToken)
                .ConfigureAwait(false);

            var dtos = users
                .Select(Map)
                .ToList()
                .AsReadOnly();

            _logger.LogInformation("Consultados {Count} usuarios", dtos.Count);
            return new SuccessUserListResponse(dtos);
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
