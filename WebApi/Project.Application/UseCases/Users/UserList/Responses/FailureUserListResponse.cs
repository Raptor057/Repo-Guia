using Common;

namespace Project.Application.UseCases.Users.UserList.Responses
{
    public record FailureUserListResponse(string Message) : UserListResponse, IFailure;
}
