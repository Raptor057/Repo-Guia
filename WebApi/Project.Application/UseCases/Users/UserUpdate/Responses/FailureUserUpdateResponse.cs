using Common;

namespace Project.Application.UseCases.Users.UserUpdate.Responses
{
    public record FailureUserUpdateResponse(string Message) : UserUpdateResponse, IFailure;
}
