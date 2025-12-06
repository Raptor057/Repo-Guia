using Common;
using Common.CleanArch;
using Project.Application.UseCases.Users.UserGet.Responses;

namespace Project.Application.UseCases.Users.UserGet
{
    public sealed class UserGetRequest : IRequest<UserGetResponse>
    {
        public static bool CanCreate(long userId, out ErrorList errors)
        {
            errors = [];
            if (userId <= 0)
            {
                errors.Add("El usuario es obligatorio.");
            }
            return errors.IsEmpty;
        }

        public static UserGetRequest Create(long userId)
        {
            if (!CanCreate(userId, out ErrorList errors))
            {
                throw errors.AsException();
            }

            return new UserGetRequest(userId);
        }

        private UserGetRequest(long userId)
        {
            UserId = userId;
        }

        public long UserId { get; }
    }
}
