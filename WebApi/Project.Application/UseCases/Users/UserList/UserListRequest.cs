using Common.CleanArch;
using Project.Application.UseCases.Users.UserList.Responses;

namespace Project.Application.UseCases.Users.UserList
{
    public sealed class UserListRequest : IRequest<UserListResponse>
    {
        public static UserListRequest Create() => new();
        private UserListRequest() { }
    }
}
