using Common;

namespace Project.Application.UseCases.Users.UserLogin.Responses
{
    public record SuccessUserLoginResponse(
        long UserId,
        string UserName,
        string FullName,
        long RoleId,
        string RoleName,
        string Message) : UserLoginResponse, ISuccess;
}
