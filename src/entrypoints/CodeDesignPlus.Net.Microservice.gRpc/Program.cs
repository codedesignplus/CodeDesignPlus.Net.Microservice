using CodeDesignPlus.Net.Microservice.gRpc.Core.FluentValidation;
using CodeDesignPlus.Net.Microservice.gRpc.Core.MediatR;
using CodeDesignPlus.Net.Microservice.gRpc.Services;
using CodeDesignPlus.Net.Mongo.Extensions;
using CodeDesignPlus.Net.Core.Extensions;
using CodeDesignPlus.Net.Redis.Extensions;
using CodeDesignPlus.Net.RabitMQ.Extensions;
using CodeDesignPlus.Net.PubSub.Extensions;
using CodeDesignPlus.Net.Observability.Extensions;
using CodeDesignPlus.Net.Logger.Extensions;
using CodeDesignPlus.Net.Security.Extensions;
using CodeDesignPlus.Net.Microservice.gRpc.Core.Interceptors;

var builder = WebApplication.CreateBuilder(args);

Serilog.Debugging.SelfLog.Enable(Console.Error);

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<ErrorInterceptor>();
});
builder.Services.AddGrpcReflection();

builder.Services.AddMapster();
builder.Services.AddMediatRR();
builder.Services.AddFluentValidation();

builder.Services.AddMongo(builder.Configuration);
builder.Services.AddCore(builder.Configuration);
builder.Services.AddRepositories<CodeDesignPlus.Net.Microservice.Infrastructure.Startup>();
builder.Services.AddPubSub(builder.Configuration);
builder.Services.AddRedis(builder.Configuration);
builder.Services.AddRabitMQ(builder.Configuration);
builder.Services.AddSecurity(builder.Configuration);

builder.Services.AddObservability(builder.Configuration);
builder.Services.AddLogger(builder.Configuration);

var app = builder.Build();

app.UseAuth();

//app.UseObservability();

// Configure the HTTP request pipeline.
app.MapGrpcService<OrdersService>().RequireAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
