using Common;
using Common.CleanArch;
using Project.Application.Dtos.Users;
using Project.Application.UseCases.Users.UserUpdate.Responses;
using System.Security.Cryptography;
using System.Text;

namespace Project.Application.UseCases.Users.UserUpdate
{
    public sealed class UserUpdateRequest : IRequest<UserUpdateResponse>
    {
        public static bool CanUpdate(UserUpdateDto dto, out ErrorList errors)
        {
            errors = [];
            ValidateUserId(dto.UserId, errors);
            ValidateFullName(dto.FullName, errors);
            ValidateUserName(dto.UserName, errors);
            ValidateRoleId(dto.RoleId, errors);
            return errors.IsEmpty;
        }

        public static UserUpdateRequest Update(UserUpdateDto dto)
        {
            if (!CanUpdate(dto, out ErrorList errors))
            {
                throw errors.AsException();
            }

            return new UserUpdateRequest(dto);
        }

        private static void ValidateUserId(long userId, ErrorList errors)
        {
            if (userId <= 0)
            {
                errors.Add("El usuario es obligatorio.");
            }
        }

        private static void ValidateFullName(string fullName, ErrorList errors)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                errors.Add("El nombre completo es obligatorio.");
            }
        }

        private static void ValidateUserName(string userName, ErrorList errors)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                errors.Add("El nombre de usuario es obligatorio.");
            }
        }

        private static void ValidateRoleId(long roleId, ErrorList errors)
        {
            if (roleId <= 0)
            {
                errors.Add("El rol es obligatorio.");
            }
        }

        private static byte[]? HashPassword(string? password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            return SHA256.HashData(Encoding.UTF8.GetBytes(password));
        }

        private UserUpdateRequest(UserUpdateDto dto)
        {
            UserId = dto.UserId;
            FullName = dto.FullName ?? string.Empty;
            UserName = dto.UserName ?? string.Empty;
            PasswordHash = HashPassword(dto.Password);
            RoleId = dto.RoleId;
        }

        public long UserId { get; }
        public string FullName { get; }
        public string UserName { get; }
        public byte[]? PasswordHash { get; }
        public long RoleId { get; }
    }
}
