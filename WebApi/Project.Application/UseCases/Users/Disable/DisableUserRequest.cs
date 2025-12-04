using Common;
using Common.CleanArch;
using Project.Application.UseCases.Users.Common;

namespace Project.Application.UseCases.Users.Disable
{
    public sealed class DisableUserRequest : IRequest<UserCommandResponse>
    {
        private DisableUserRequest(int userId)
        {
            UserId = userId;
        }

        public int UserId { get; }

        public static bool CanDisable(int userId, out ErrorList errors)
        {
            errors = [];
            if (userId <= 0)
            {
                errors.Add("El identificador del usuario es obligatorio.");
            }

            return errors.IsEmpty;
        }

        public static DisableUserRequest Create(int userId)
        {
            if (!CanDisable(userId, out var errors)) throw errors.AsException();
            return new DisableUserRequest(userId);
        }
    }
}
