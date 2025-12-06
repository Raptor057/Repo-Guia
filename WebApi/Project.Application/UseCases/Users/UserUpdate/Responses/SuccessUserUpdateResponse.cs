using Common;

namespace Project.Application.UseCases.Users.UserUpdate.Responses
{
    public record SuccessUserUpdateResponse(string Message) : UserUpdateResponse, ISuccess;
}
