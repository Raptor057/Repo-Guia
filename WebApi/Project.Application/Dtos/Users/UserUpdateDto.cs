namespace Project.Application.Dtos.Users
{
    public record UserUpdateDto(
        long UserId,
        string FullName,
        string UserName,
        string? Password,
        long RoleId
    );
}
