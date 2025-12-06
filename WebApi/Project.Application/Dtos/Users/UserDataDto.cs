namespace Project.Application.Dtos.Users
{
    public record UserDataDto(
        long UserId,
        string FullName,
        string UserName,
        long RoleId,
        bool IsActive,
        DateTime UtcTimeStamp
    );
}
