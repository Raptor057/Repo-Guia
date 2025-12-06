using Common;
using Project.Application.Dtos.Users;

namespace Project.Application.UseCases.Users.UserGet.Responses
{
    public record SuccessUserGetResponse(UserDataDto Data) : UserGetResponse, ISuccess<UserDataDto>;
}
