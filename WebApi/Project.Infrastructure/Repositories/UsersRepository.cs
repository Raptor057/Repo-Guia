using Project.Domain.IRepositories;
using Project.Infrastructure.DataSources.SqlDB;
using UserEntity = Project.Domain.Entities.User;

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
            _ = cancellationToken;
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

        public async Task<Project.Domain.Entities.AuthUser?> GetUserForLoginAsync(
            string username,
            string password,
            CancellationToken cancellationToken)
        {
            _ = cancellationToken;
            const string sql = @"
                SELECT
                    u.UserID        AS UserId,
                    u.UserName      AS UserName,
                    u.UserFullName  AS UserFullName,
                    u.UserRolID     AS RoleId,
                    r.Rol           AS RoleName,
                    u.UserActive    AS UserActive
                FROM Users u
                INNER JOIN SystemRoles r ON r.RolID = u.UserRolID
                WHERE u.UserName = @UserName
                  AND CONVERT(VARCHAR(64), u.PasswordHash, 2) = @Password
                  AND u.UserActive = 1;
            ";

            var user = await _db.QueryFirstAsync<Project.Domain.Entities.AuthUser?>(sql, new
            {
                UserName = username,
                Password = password
            }).ConfigureAwait(false);

            return user;
        }

        public async Task<bool> UserNameExistsAsync(
            string username,
            long? excludeUserId,
            CancellationToken cancellationToken)
        {
            _ = cancellationToken;
            const string sql = @"
                SELECT COUNT(1)
                FROM Users
                WHERE UserName = @UserName
                  AND (@ExcludeUserId IS NULL OR UserID <> @ExcludeUserId);
            ";

            var count = await _db.ExecuteScalarAsync<int>(sql, new
            {
                UserName = username,
                ExcludeUserId = excludeUserId
            }).ConfigureAwait(false);

            return count > 0;
        }

        public async Task<UserEntity?> GetByIdAsync(long userId, CancellationToken cancellationToken)
        {
            _ = cancellationToken;
            const string sql = @"
                SELECT
                    UserID      AS UserId,
                    UserFullName,
                    UserName,
                    UserRolID   AS RoleId,
                    UserActive,
                    UtcTimeStamp
                FROM Users
                WHERE UserID = @UserId;
            ";

            var user = await _db.QueryFirstAsync<UserEntity?>(sql, new
            {
                UserId = userId
            }).ConfigureAwait(false);

            return user;
        }

        public async Task<IReadOnlyCollection<UserEntity>> GetListAsync(CancellationToken cancellationToken)
        {
            _ = cancellationToken;
            const string sql = @"
                SELECT
                    UserID      AS UserId,
                    UserFullName,
                    UserName,
                    UserRolID   AS RoleId,
                    UserActive,
                    UtcTimeStamp
                FROM Users
                ORDER BY UserFullName;
            ";

            var users = await _db.QueryAsync<UserEntity>(sql).ConfigureAwait(false);
            return users.ToList();
        }

        public async Task<bool> UserExistsAsync(long userId, CancellationToken cancellationToken)
        {
            _ = cancellationToken;
            const string sql = @"
                SELECT COUNT(1)
                FROM Users
                WHERE UserID = @UserId;
            ";

            var count = await _db.ExecuteScalarAsync<int>(sql, new
            {
                UserId = userId
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
            _ = cancellationToken;
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

        public async Task<bool> UpdateUserAsync(
            long userId,
            string fullName,
            string username,
            byte[]? passwordHash,
            long roleId,
            CancellationToken cancellationToken)
        {
            _ = cancellationToken;
            const string sql = @"
                UPDATE Users
                SET UserFullName = @FullName,
                    UserName = @UserName,
                    UserRolID = @RoleId,
                    PasswordHash = CASE WHEN @PasswordHash IS NULL THEN PasswordHash ELSE @PasswordHash END
                WHERE UserID = @UserId;
            ";

            var affectedRows = await _db.ExecuteAsync(sql, new
            {
                UserId = userId,
                FullName = fullName,
                UserName = username,
                RoleId = roleId,
                PasswordHash = passwordHash
            }).ConfigureAwait(false);

            return affectedRows > 0;
        }

        public async Task<bool> UpdateUserActiveAsync(long userId, bool isActive, CancellationToken cancellationToken)
        {
            _ = cancellationToken;
            const string sql = @"
                UPDATE Users
                SET UserActive = @IsActive
                WHERE UserID = @UserId;
            ";

            var affectedRows = await _db.ExecuteAsync(sql, new
            {
                UserId = userId,
                IsActive = isActive
            }).ConfigureAwait(false);

            return affectedRows > 0;
        }
    }
}
