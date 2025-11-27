namespace Project.Domain.IRepositories
{
    public interface IUsersRepository
    {
        Task<bool> IsValidUserCredentialsAsync(string username, string password, CancellationToken cancellationToken);
        
    }
}
