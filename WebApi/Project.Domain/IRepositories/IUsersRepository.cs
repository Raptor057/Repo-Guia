namespace Project.Domain.IRepositories
{
    public interface IUsersRepository
    {
        Task<bool> IsValidUserCredentialsAsync(string username, string password, CancellationToken cancellationToken);
        Task<bool> UserNameExistsAsync(string username, CancellationToken cancellationToken);
        Task<long> CreateUserAsync(
            string fullName,
            string username,
            byte[] passwordHash,
            long roleId,
            CancellationToken cancellationToken);

    }
}
