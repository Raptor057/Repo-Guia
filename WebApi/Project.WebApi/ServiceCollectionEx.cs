using Common.CleanArch;
using Project.Application;
using Project.Infrastructure;
using Project.WebApi.Controllers.Users;

public static class ServiceCollectionEx
{
    public static IServiceCollection AddWebApiServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddHealthChecks();

        services.AddInfraServices(configuration);
        services.AddAppServices(); //IMPORTANTE

        services.AddScoped(typeof(ResultViewModel<>));
        services.AddScoped(typeof(GenericViewModel<>));

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(UsersController).Assembly);
        });

        return services;
    }
}
