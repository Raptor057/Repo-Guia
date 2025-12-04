namespace Project.Domain.Entities
{
    public sealed class User
    {
        public int Id { get; init; }
        public string UserName { get; init; } = string.Empty;
        public string PasswordHash { get; init; } = string.Empty;
        public string FullName { get; init; } = string.Empty;
        public bool UserActive { get; init; }
    }
}
