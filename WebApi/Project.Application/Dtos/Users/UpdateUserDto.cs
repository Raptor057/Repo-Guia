namespace Project.Application.Dtos.Users
{
    public record UpdateUserDto(int UserId, string? Password, string? FullName);
}
