using Common;
using Common.CleanArch;
using Project.Application.Dtos.Users;
using Project.Application.UseCases.Users.Common;

namespace Project.Application.UseCases.Users.Create
{
    public sealed class CreateUserRequest : IRequest<UserCommandResponse>
    {
        private CreateUserRequest(CreateUserDto dto)
        {
            UserName = dto.UserName.Trim();
            Password = UserPasswordHasher.Hash(dto.Password);
            FullName = dto.FullName.Trim();
        }

        public string UserName { get; }
        public string Password { get; }
        public string FullName { get; }

        public static CreateUserRequest Create(CreateUserDto dto)
        {
            if (!CanCreate(dto, out var errors)) throw errors.AsException();
            return new CreateUserRequest(dto);
        }

        public static bool CanCreate(CreateUserDto dto, out ErrorList errors)
        {
            errors = [];
            if (string.IsNullOrWhiteSpace(dto.UserName)) errors.Add("El usuario es obligatorio.");
            if (string.IsNullOrWhiteSpace(dto.Password)) errors.Add("La contraseña es obligatoria.");
            if (string.IsNullOrWhiteSpace(dto.FullName)) errors.Add("El nombre es obligatorio.");
            return errors.IsEmpty;
        }
    }
}
