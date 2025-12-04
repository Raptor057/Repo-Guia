using Common;

namespace Project.Application.UseCases.Users.UserLogin.Responses
{
    public record FailureUserLoginResponse(string Message, int StatusCode = 401) : UserLoginResponse, IFailure
    {
        public int StatusCode { get; init; } = StatusCode;
    }
}
