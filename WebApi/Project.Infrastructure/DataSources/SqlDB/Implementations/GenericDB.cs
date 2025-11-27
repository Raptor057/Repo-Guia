namespace Project.Infrastructure.DataSources.SqlDB.Implementations
{
    internal class GenericDB<T> : DapperSqlDbConnection, IGenericDB<T>
    {
        public GenericDB(GenericConfigurationSqlDbConnectionFactory<T> connections)
            : base(connections.CreateOpenConnection())
        { }
    }
}
