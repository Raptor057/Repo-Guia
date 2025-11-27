using System.Data;

namespace Project.Infrastructure.DataSources.SqlDB
{
    public interface ISqlDbConnectionFactory
    {
        /// <summary>
        /// Return an open instance of the IDbConnection.
        /// </summary>
        Task<IDbConnection> CreateOpenConnectionAsync();

        /// <summary>
        /// Return an open instance of the IDbConnection.
        /// </summary>
        IDbConnection CreateOpenConnection();
    }
}
