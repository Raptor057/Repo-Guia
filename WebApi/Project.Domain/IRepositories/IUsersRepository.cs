namespace Project.Domain.IRepositories
{
    public interface IUsersRepository
    {
        Task<bool> IsValidUserCredentialsAsync(string username, string password, CancellationToken cancellationToken);
        Task<bool> UserNameExistsAsync(string username, long? excludeUserId, CancellationToken cancellationToken);
        Task<bool> UserExistsAsync(long userId, CancellationToken cancellationToken);
        Task<Entities.User?> GetByIdAsync(long userId, CancellationToken cancellationToken);
        Task<IReadOnlyCollection<Entities.User>> GetListAsync(CancellationToken cancellationToken);
        Task<long> CreateUserAsync(
            string fullName,
            string username,
            byte[] passwordHash,
            long roleId,
            CancellationToken cancellationToken);
        Task<bool> UpdateUserAsync(
            long userId,
            string fullName,
            string username,
            byte[]? passwordHash,
            long roleId,
            CancellationToken cancellationToken);
        Task<bool> UpdateUserActiveAsync(long userId, bool isActive, CancellationToken cancellationToken);

    }
}
