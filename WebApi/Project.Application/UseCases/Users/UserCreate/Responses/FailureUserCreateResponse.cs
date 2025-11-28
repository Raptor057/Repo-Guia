using Common;


namespace Project.Application.UseCases.Users.UserCreate.Responses
{
    public record FailureUserCreateResponse(string Message)
        : UserCreateResponse, IFailure;
}
