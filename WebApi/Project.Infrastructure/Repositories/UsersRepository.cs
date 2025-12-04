using Project.Domain.Entities;
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

        public async Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken)
        {
            const string sql = @"SELECT COUNT(1) FROM Users WHERE UserName = @UserName;";
            var count = await _db.ExecuteScalarAsync<int>(sql, new { UserName = username }).ConfigureAwait(false);
            return count > 0;
        }

        public async Task<int> CreateUserAsync(User user, CancellationToken cancellationToken)
        {
            const string sql = @"
                INSERT INTO Users (UserName, PasswordHash, FullName, UserActive)
                VALUES (@UserName, CONVERT(VARBINARY(32), @PasswordHash, 2), @FullName, 1);
                SELECT CAST(SCOPE_IDENTITY() AS INT);
            ";

            var id = await _db.ExecuteScalarAsync<int>(sql, new
            {
                user.UserName,
                user.PasswordHash,
                user.FullName
            }).ConfigureAwait(false);

            return id;
        }

        public async Task<User?> GetByIdAsync(int userId, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT
                    Id = ISNULL(UserId, IdUser),
                    UserName,
                    PasswordHash = CONVERT(VARCHAR(64), PasswordHash, 2),
                    FullName,
                    UserActive
                FROM Users
                WHERE (UserId = @UserId OR IdUser = @UserId);
            ";

            var user = await _db.QueryFirstAsync<User>(sql, new { UserId = userId }).ConfigureAwait(false);
            return user;
        }

        public async Task<bool> DisableUserAsync(int userId, CancellationToken cancellationToken)
        {
            const string sql = @"
                UPDATE Users
                SET UserActive = 0
                WHERE (UserId = @UserId OR IdUser = @UserId)
                  AND UserActive = 1;
            ";

            var rows = await _db.ExecuteAsync(sql, new { UserId = userId }).ConfigureAwait(false);
            return rows > 0;
        }

        public async Task<bool> UpdateUserAsync(int userId, string? fullName, string? passwordHash, CancellationToken cancellationToken)
        {
            var setClauses = new List<string>();
            if (!string.IsNullOrWhiteSpace(fullName)) setClauses.Add("FullName = @FullName");
            if (!string.IsNullOrWhiteSpace(passwordHash)) setClauses.Add("PasswordHash = CONVERT(VARBINARY(32), @PasswordHash, 2)");
            if (setClauses.Count == 0) return false;

            var sql = $@"UPDATE Users SET {string.Join(", ", setClauses)} WHERE (UserId = @UserId OR IdUser = @UserId) AND UserActive = 1;";
            var rows = await _db.ExecuteAsync(sql, new { UserId = userId, FullName = fullName, PasswordHash = passwordHash }).ConfigureAwait(false);
            return rows > 0;
        }
    }
}
