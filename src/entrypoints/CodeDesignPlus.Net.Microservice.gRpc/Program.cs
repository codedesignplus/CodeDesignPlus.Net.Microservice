using CodeDesignPlus.Net.Logger.Extensions;
using CodeDesignPlus.Net.Microservice.Commons.EntryPoints.gRpc.Interceptors;
using CodeDesignPlus.Net.Microservice.Commons.FluentValidation;
using CodeDesignPlus.Net.Microservice.Commons.MediatR;
using CodeDesignPlus.Net.Microservice.gRpc.Services;
using CodeDesignPlus.Net.Mongo.Extensions;
using CodeDesignPlus.Net.Observability.Extensions;
using CodeDesignPlus.Net.RabbitMQ.Extensions;
using CodeDesignPlus.Net.Redis.Extensions;
using CodeDesignPlus.Net.Security.Extensions;
using CodeDesignPlus.Net.Vault.Extensions;

var builder = WebApplication.CreateBuilder(args);

Serilog.Debugging.SelfLog.Enable(Console.Error);

builder.Host.UseSerilog();

builder.Configuration.AddVault();

// Add services to the container.
builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<ErrorInterceptor>();
});
builder.Services.AddGrpcReflection();

builder.Services.AddVault(builder.Configuration);
builder.Services.AddMapster();
builder.Services.AddMediatR<CodeDesignPlus.Net.Microservice.Application.Startup>();
builder.Services.AddFluentValidation();

builder.Services.AddMongo<CodeDesignPlus.Net.Microservice.Infrastructure.Startup>(builder.Configuration);
builder.Services.AddRedis(builder.Configuration);
builder.Services.AddRabbitMQ<CodeDesignPlus.Net.Microservice.Domain.Startup>(builder.Configuration);
builder.Services.AddSecurity(builder.Configuration);
builder.Services.AddObservability(builder.Configuration, builder.Environment);
builder.Services.AddLogger(builder.Configuration);

var app = builder.Build();

app.UseAuth();

// Configure the HTTP request pipeline.
app.MapGrpcService<OrdersService>().RequireAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

await app.RunAsync();

public partial class Program
{
    protected Program() { }
}