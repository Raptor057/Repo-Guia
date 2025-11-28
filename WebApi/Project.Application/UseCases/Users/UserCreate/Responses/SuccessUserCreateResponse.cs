using Common;

namespace Project.Application.UseCases.Users.UserCreate.Responses
{
    public record SuccessUserCreateResponse(long UserId, string Message)
        : UserCreateResponse, ISuccess;
}
