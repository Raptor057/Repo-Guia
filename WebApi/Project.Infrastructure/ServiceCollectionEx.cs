using Common.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Project.Domain.IRepositories;
using Project.Infrastructure.DataSources.SqlDB;
using Project.Infrastructure.DataSources.SqlDB.Implementations;
using Project.Infrastructure.Repositories;

namespace Project.Infrastructure
{
    public static class ServiceCollectionEx
    {
        public static IServiceCollection AddInfraServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Logging centralizado (Seq, etc.)
            var configRoot = (IConfigurationRoot)configuration;
            services.AddLoggingServices(configRoot);

            // Fábrica genérica por tipo: usa connection string con el nombre del tipo (Project, etc.)
            services.AddSingleton(typeof(GenericConfigurationSqlDbConnectionFactory<>));

            // Wrapper Dapper genérico
            services.AddScoped(typeof(IGenericDB<>), typeof(GenericDB<>));

            // DB tipada para Project
            services.AddScoped<IProjectDb, ProjectDB>();

            //Repo de usuarios
            services.AddScoped<IUsersRepository, UsersRepository>();

            return services;
        }
    }
}
