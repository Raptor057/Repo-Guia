using Common;
using Common.CleanArch;
using Project.Application.Dtos.Users;
using Project.Application.UseCases.Users.Common;

namespace Project.Application.UseCases.Users.Update
{
    public sealed class UpdateUserRequest : IRequest<UserCommandResponse>
    {
        private UpdateUserRequest(UpdateUserDto dto)
        {
            UserId = dto.UserId;
            Password = string.IsNullOrWhiteSpace(dto.Password) ? null : UserPasswordHasher.Hash(dto.Password);
            FullName = string.IsNullOrWhiteSpace(dto.FullName) ? null : dto.FullName.Trim();
        }

        public int UserId { get; }
        public string? Password { get; }
        public string? FullName { get; }

        public static bool CanUpdate(UpdateUserDto dto, out ErrorList errors)
        {
            errors = [];
            if (dto.UserId <= 0) errors.Add("El identificador del usuario es obligatorio.");
            if (string.IsNullOrWhiteSpace(dto.Password) && string.IsNullOrWhiteSpace(dto.FullName))
            {
                errors.Add("Se debe indicar al menos un campo a modificar.");
            }
            return errors.IsEmpty;
        }

        public static UpdateUserRequest Create(UpdateUserDto dto)
        {
            if (!CanUpdate(dto, out var errors)) throw errors.AsException();
            return new UpdateUserRequest(dto);
        }
    }
}
