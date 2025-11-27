using Common.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Project.Infrastructure
{
    public static class ServiceCollectionEx
    {
        public static IServiceCollection AddInfraServices(this IServiceCollection services, IConfigurationRoot configuration)
        {
            return services
                .AddLoggingServices(configuration);
                //.AddSingleton(typeof(ConfigurationSqlDbConnectionFactory<>))
                //.AddSingleton(typeof(ConfigurationSqlDbConnection<>));
        }
    }
}
