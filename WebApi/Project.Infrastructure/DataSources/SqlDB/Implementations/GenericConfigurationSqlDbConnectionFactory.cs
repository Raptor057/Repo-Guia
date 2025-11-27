using Microsoft.Extensions.Configuration;


namespace Project.Infrastructure.DataSources.SqlDB.Implementations
{
    internal class GenericConfigurationSqlDbConnectionFactory<T> : ConnectionStringSqlDbConnectionFactory
    {
        /// <summary>
        /// Get the connection string from the configuration file named after the generic parameter type
        /// and passes it to the base class.
        /// </summary>
        public GenericConfigurationSqlDbConnectionFactory(IConfiguration config)
            : base(config.GetConnectionString(typeof(T).Name) ?? throw new KeyNotFoundException($"Cadena de conexión \"{typeof(T).Name}\" no encontrada o en blanco."))
        { }
    }
}
