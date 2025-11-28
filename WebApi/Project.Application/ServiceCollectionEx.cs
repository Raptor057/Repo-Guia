using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Common.CleanArch;

namespace Project.Application
{
    public static class ServiceCollectionEx
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            return services
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(InteractorPipeline<,>))
                .AddMediatR(cfg =>
                {
                    cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionEx).Assembly);
                });
        }
    }
}
