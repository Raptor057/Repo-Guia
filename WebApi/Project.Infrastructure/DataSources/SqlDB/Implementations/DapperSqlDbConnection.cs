using Dapper;
using System.Data;


namespace Project.Infrastructure.DataSources.SqlDB.Implementations
{
    internal abstract class DapperSqlDbConnection : ISqlDbConnection
    {
        private readonly IDbConnection _connection;

        protected DapperSqlDbConnection(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? args = null) =>
            await _connection.QueryAsync<T>(sql, args).ConfigureAwait(false);


        public async Task<T> QuerySingleAsync<T>(string sql, object? args = null) =>
            await _connection.QuerySingleOrDefaultAsync<T>(sql, args).ConfigureAwait(false);


        public async Task<T> QueryFirstAsync<T>(string sql, object? args = null) =>
            await _connection.QueryFirstOrDefaultAsync<T>(sql, args).ConfigureAwait(false);

        public async Task<T> ExecuteScalarAsync<T>(string sql, object? args = null) =>
            await _connection.ExecuteScalarAsync<T>(sql, args).ConfigureAwait(false);

        public async Task<int> ExecuteAsync(string sql, object? args = null) =>
            await _connection.ExecuteAsync(sql, args).ConfigureAwait(false);

        public IEnumerable<T> Query<T>(string sql, object? args = null) =>
            QueryAsync<T>(sql, args).GetAwaiter().GetResult();

        public T QuerySingle<T>(string sql, object? args = null) =>
            QuerySingleAsync<T>(sql, args).GetAwaiter().GetResult();

        public T QueryFirst<T>(string sql, object? args = null) =>
            QueryFirstAsync<T>(sql, args).GetAwaiter().GetResult();

        public T ExecuteScalar<T>(string sql, object? args = null) =>
            ExecuteScalarAsync<T>(sql, args).GetAwaiter().GetResult();

        public int Execute(string sql, object? args = null) =>
            ExecuteAsync(sql, args).GetAwaiter().GetResult();
        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
