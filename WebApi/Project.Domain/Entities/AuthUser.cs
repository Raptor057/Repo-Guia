namespace Project.Domain.Entities
{
    public sealed class AuthUser
    {
        public long UserId { get; init; }
        public string UserName { get; init; } = string.Empty;
        public string UserFullName { get; init; } = string.Empty;
        public long RoleId { get; init; }
        public string RoleName { get; init; } = string.Empty;
        public bool UserActive { get; init; }
    }
}
