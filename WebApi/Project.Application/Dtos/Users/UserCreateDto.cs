namespace Project.Application.Dtos.Users
{
    public record UserCreateDto(
        string FullName,
        string UserName,
        string Password,
        long RoleId
    );
}
