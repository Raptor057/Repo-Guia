using MediatR;
using Prometheus;
using Common.CleanArch;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Project.Application;
using Project.Infrastructure;
using Project.WebApi.Controllers.Users;

var builder = WebApplication.CreateBuilder(args);

// Servicios básicos WebApi
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddWebApiServices(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = false
    };
});

// CLEAN ARCH
builder.Services.AddInfraServices(builder.Configuration); // Dapper, repos, logging
builder.Services.AddAppServices();                        // MediatR + InteractorPipeline

// ViewModels para presenters (por request)
builder.Services.AddScoped(typeof(ResultViewModel<>));
builder.Services.AddScoped(typeof(GenericViewModel<>));

// MediatR adicional para presenters que viven en el assembly WebApi
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(UsersController).Assembly);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// Middleware de Prometheus para m�tricas HTTP (latencia, status codes, etc.)
app.UseHttpMetrics();

app.UseAuthentication();
app.UseAuthorization();

// ---- ENDPOINTS EXTRA ----

// 1) /env -> ver entorno
app.MapGet("/api/env", (IHostEnvironment env) =>
{
    return Results.Ok(new
    {
        environment = env.EnvironmentName,  // Development, Staging, Production, etc.
        application = env.ApplicationName
    });
});

// 2) /version -> lee el archivo VERSION del output
app.MapGet("/api/version", () =>
{
    var versionFilePath = Path.Combine(AppContext.BaseDirectory, "VERSION");

    if (!File.Exists(versionFilePath))
    {
        return Results.NotFound(new
        {
            error = "VERSION file not found",
            path = versionFilePath
        });
    }

    var version = File.ReadAllText(versionFilePath).Trim();
    return Results.Ok(new { version });
});

// 3) /health -> healthcheck b�sico
app.MapHealthChecks("/api/health");

// 4) /metrics -> endpoint de Prometheus
app.MapMetrics("/api/metrics");

// Controllers existentes
app.MapControllers();

app.Run();
