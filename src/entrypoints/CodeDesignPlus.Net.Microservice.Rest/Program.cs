using CodeDesignPlus.Net.Microservice.Commons.EntryPoints.Rest.Middlewares;
using CodeDesignPlus.Net.Microservice.Commons.EntryPoints.Rest.Swagger;
using CodeDesignPlus.Net.Microservice.Commons.FluentValidation;
using CodeDesignPlus.Net.Microservice.Commons.MediatR;
using CodeDesignPlus.Net.Mongo.Abstractions.Options;
using CodeDesignPlus.Net.RabbitMQ.Abstractions;
using CodeDesignPlus.Net.RabbitMQ.Abstractions.Options;
using CodeDesignPlus.Net.Redis.Abstractions;
using CodeDesignPlus.Net.Redis.Cache.Extensions;
using CodeDesignPlus.Net.Redis.Options;
using CodeDesignPlus.Net.Vault.Extensions;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Driver;
using NodaTime.Serialization.SystemTextJson;


var builder = WebApplication.CreateSlimBuilder(args);

Serilog.Debugging.SelfLog.Enable(Console.Error);

builder.Host.UseSerilog();

builder.Configuration.AddVault();

builder.Services
    .AddControllers()
    .AddJsonOptions(opt => opt.JsonSerializerOptions.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddVault(builder.Configuration);
builder.Services.AddRedis(builder.Configuration);
builder.Services.AddMongo<CodeDesignPlus.Net.Microservice.Infrastructure.Startup>(builder.Configuration);
builder.Services.AddObservability(builder.Configuration, builder.Environment);
builder.Services.AddLogger(builder.Configuration);
builder.Services.AddRabbitMQ<CodeDesignPlus.Net.Microservice.Domain.Startup>(builder.Configuration);
builder.Services.AddMapster();
builder.Services.AddFluentValidation();
builder.Services.AddMediatR<CodeDesignPlus.Net.Microservice.Application.Startup>();
builder.Services.AddSecurity(builder.Configuration);
builder.Services.AddCoreSwagger<Program>(builder.Configuration);
builder.Services.AddCache(builder.Configuration);
builder.Services.AddHealthChecks()
    .AddRedis(x =>
    {
        var factory = x.GetRequiredService<IRedisFactory>();

        return factory.Create(FactoryConst.RedisCore).Connection;
    }, name: "Redis", tags: ["ready"])
    .AddRabbitMQ(x =>
    {
        var raabbitConnection = x.GetRequiredService<IRabbitConnection>();

        return raabbitConnection.Connection;
    }, name: "RabbitMQ", tags: ["ready"])
    .AddMongoDb(x =>
    {
        var mongoClient = x.GetRequiredService<IMongoClient>();

        return mongoClient;
    }, name: "MongoDB", tags: ["ready"])
    .AddCheck("self", () => HealthCheckResult.Healthy(), tags: ["live"]);


var app = builder.Build();

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = healthCheck => healthCheck.Tags.Contains("ready")
});

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = healthCheck => healthCheck.Tags.Contains("live"), // Solo checks con el tag "live"
});
app.UseCoreSwagger();

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuth();

app.MapControllers().RequireAuthorization();

await app.RunAsync();

public partial class Program
{
    protected Program() { }
}