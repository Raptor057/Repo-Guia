using Common;

namespace Project.Application.UseCases.Users.UserDisable.Responses
{
    public record SuccessUserDisableResponse(string Message) : UserDisableResponse, ISuccess;
}
