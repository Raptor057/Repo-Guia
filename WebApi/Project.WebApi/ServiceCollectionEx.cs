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
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new() { Title = "Project.WebApi", Version = "v1" });
            options.AddSecurityDefinition("Bearer", new()
            {
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Description = "Token JWT en el header Authorization. Ejemplo: 'Bearer {token}'"
            });
            options.AddSecurityRequirement(new()
            {
                {
                    new()
                    {
                        Reference = new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
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
