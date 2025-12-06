using Common;
using Project.Application.Dtos.Users;

namespace Project.Application.UseCases.Users.UserList.Responses
{
    public record SuccessUserListResponse(IReadOnlyCollection<UserDataDto> Data) : UserListResponse, ISuccess<IReadOnlyCollection<UserDataDto>>;
}
