using Common;

namespace Project.Application.UseCases.Users.UserLogin.Responses
{
    public record SuccessUserLoginResponse(string Message) : UserLoginResponse, ISuccess;
}
