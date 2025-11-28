using Project.Domain.IRepositories;
using Project.Infrastructure.DataSources.SqlDB;

namespace Project.Infrastructure.Repositories
{
    internal sealed class UsersRepository : IUsersRepository
    {
        private readonly IProjectDb _db;

        public UsersRepository(IProjectDb db)
        {
            _db = db;
        }

        public async Task<bool> IsValidUserCredentialsAsync(
            string username,
            string password,
            CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT COUNT(1)
                FROM Users
                WHERE UserName = @UserName
                  AND CONVERT(VARCHAR(64), PasswordHash, 2) = @Password
                  AND UserActive = 1;
            ";

            var count = await _db.ExecuteScalarAsync<int>(sql, new
            {
                UserName = username,
                Password = password
            }).ConfigureAwait(false);

            return count > 0;
        }

        public async Task<bool> UserNameExistsAsync(
            string username,
            CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT COUNT(1)
                FROM Users
                WHERE UserName = @UserName;
            ";

            var count = await _db.ExecuteScalarAsync<int>(sql, new
            {
                UserName = username
            }).ConfigureAwait(false);

            return count > 0;
        }

        public async Task<long> CreateUserAsync(
            string fullName,
            string username,
            byte[] passwordHash,
            long roleId,
            CancellationToken cancellationToken)
        {
            const string sql = @"
                INSERT INTO Users (UserFullName, UserName, PasswordHash, UserRolID)
                OUTPUT INSERTED.UserID
                VALUES (@FullName, @UserName, @PasswordHash, @RoleId);
            ";

            var id = await _db.ExecuteScalarAsync<long>(sql, new
            {
                FullName = fullName,
                UserName = username,
                PasswordHash = passwordHash,
                RoleId = roleId
            }).ConfigureAwait(false);

            return id;
        }
    }
}
