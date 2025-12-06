using Common;

namespace Project.Application.UseCases.Users.UserDisable.Responses
{
    public record FailureUserDisableResponse(string Message) : UserDisableResponse, IFailure;
}
