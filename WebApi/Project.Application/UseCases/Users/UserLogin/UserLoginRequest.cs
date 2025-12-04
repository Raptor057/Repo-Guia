using Common;
using Common.CleanArch;
using Project.Application.Dtos.Users;
using Project.Application.UseCases.Users.UserLogin.Responses;

namespace Project.Application.UseCases.Users.UserLogin
{
    public sealed class UserLoginRequest : IRequest<UserLoginResponse>
    {
        public static bool CanLoggin(UserLoginDto loginUserDto, out ErrorList errors)
        {
            errors = [];
            ValidateUser(loginUserDto.Usr, errors);
            ValidatePassword(loginUserDto.Psswd, errors);
            return errors.IsEmpty;
        }

        public static UserLoginRequest Login(UserLoginDto loginUserDto)
        {
            if (!CanLoggin(loginUserDto, out ErrorList errors)) throw errors.AsException();
            return new UserLoginRequest(loginUserDto);
        }

        private static void ValidateUser(string User, ErrorList errors)
        {
            if (string.IsNullOrWhiteSpace(User))
            {
                errors.Add("El usuario es obligatorio");
            }
        }
        private static void ValidatePassword(string Password, ErrorList errors)
        {
            if (string.IsNullOrWhiteSpace(Password))
            {
                errors.Add("La contraseña es obligatoria");
            }
        }

        private UserLoginRequest(UserLoginDto loginUserDto)
        {
            User = loginUserDto.Usr ?? string.Empty;
            Password = UserPasswordHasher.Hash(loginUserDto.Psswd ?? string.Empty);
        }

        public string User { get; }
        public string Password { get; }
    }
}
