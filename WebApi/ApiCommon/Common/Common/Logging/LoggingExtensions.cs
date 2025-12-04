using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Common.Logging
{
    public static class LoggingExtensions
    {
        public static IServiceCollection AddLoggingServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging(builder =>
            {
                builder.AddConsole();
            });
            return services;
        }
    }
}
