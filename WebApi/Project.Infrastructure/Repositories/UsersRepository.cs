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
            // password aquí es la cadena HEX (SHA256) que genera UserLoginRequest
            const string sql = @"
                SELECT COUNT(1)
                FROM Users
                WHERE UserName = @UserName
                  AND CONVERT(VARCHAR(64), PasswordHash, 2) = @Password
                  AND UserActive = 1;
            ";

            // Nuestro wrapper Dapper no recibe CancellationToken, así que por ahora no lo usamos aquí
            var count = await _db.ExecuteScalarAsync<int>(sql, new
            {
                UserName = username,
                Password = password
            }).ConfigureAwait(false);

            return count > 0;
        }
    }
}
