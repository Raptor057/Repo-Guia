using Common;
using Common.CleanArch;
using Project.Application.Dtos.Users;
using Project.Application.UseCases.Users.UserCreate.Responses;
using System.Security.Cryptography;
using System.Text;

namespace Project.Application.UseCases.Users.UserCreate
{
    public sealed class UserCreateRequest : IRequest<UserCreateResponse>
    {
        public static bool CanCreate(UserCreateDto dto, out ErrorList errors)
        {
            errors = [];
            ValidateFullName(dto.FullName, errors);
            ValidateUserName(dto.UserName, errors);
            ValidatePassword(dto.Password, errors);
            ValidateRoleId(dto.RoleId, errors);
            return errors.IsEmpty;
        }

        public static UserCreateRequest Create(UserCreateDto dto)
        {
            if (!CanCreate(dto, out ErrorList errors))
            {
                throw errors.AsException();
            }

            return new UserCreateRequest(dto);
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

        private static void ValidatePassword(string password, ErrorList errors)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                errors.Add("La contraseña es obligatoria.");
            }
        }

        private static void ValidateRoleId(long roleId, ErrorList errors)
        {
            if (roleId <= 0)
            {
                errors.Add("El rol es obligatorio.");
            }
        }

        private static byte[] HashPassword(string password)
        {
            return SHA256.HashData(Encoding.UTF8.GetBytes(password));
        }

        private UserCreateRequest(UserCreateDto dto)
        {
            FullName = dto.FullName ?? string.Empty;
            UserName = dto.UserName ?? string.Empty;
            PasswordHash = HashPassword(dto.Password ?? string.Empty);
            RoleId = dto.RoleId;
        }

        public string FullName { get; }
        public string UserName { get; }
        public byte[] PasswordHash { get; }
        public long RoleId { get; }
    }
}
