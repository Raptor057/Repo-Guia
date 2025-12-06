using Common;
using Common.CleanArch;
using Project.Application.Dtos.Users;
using Project.Application.UseCases.Users.UserDisable.Responses;

namespace Project.Application.UseCases.Users.UserDisable
{
    public sealed class UserDisableRequest : IRequest<UserDisableResponse>
    {
        public static bool CanDisable(UserDisableDto dto, out ErrorList errors)
        {
            errors = [];
            ValidateUserId(dto.UserId, errors);
            // No validation needed for IsActive (bool)
            return errors.IsEmpty;
        }

        public static UserDisableRequest Disable(UserDisableDto dto)
        {
            if (!CanDisable(dto, out ErrorList errors))
            {
                throw errors.AsException();
            }

            return new UserDisableRequest(dto.UserId, dto.IsActive);
        }

        private static void ValidateUserId(long userId, ErrorList errors)
        {
            if (userId <= 0)
            {
                errors.Add("El usuario es obligatorio.");
            }
        }

        private UserDisableRequest(long userId, bool isActive)
        {
            UserId = userId;
            IsActive = isActive;
        }

        public long UserId { get; }
        public bool IsActive { get; }
    }
}
