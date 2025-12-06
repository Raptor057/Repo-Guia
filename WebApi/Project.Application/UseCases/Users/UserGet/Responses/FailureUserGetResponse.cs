using Common;

namespace Project.Application.UseCases.Users.UserGet.Responses
{
    public record FailureUserGetResponse(string Message) : UserGetResponse, IFailure;
}
