
using Common;

namespace Project.Application.UseCases.Users.UserLogin.Responses
{
    public record FailureUserLoginResponse(string Message) : UserLoginResponse, IFailure;
}
