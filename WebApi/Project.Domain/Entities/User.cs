namespace Project.Domain.Entities
{
    public sealed class User
    {
        public long UserId { get; init; }
        public string UserFullName { get; init; } = string.Empty;
        public string UserName { get; init; } = string.Empty;
        public long RoleId { get; init; }
        public bool UserActive { get; init; }
        public DateTime UtcTimeStamp { get; init; }
    }
}
