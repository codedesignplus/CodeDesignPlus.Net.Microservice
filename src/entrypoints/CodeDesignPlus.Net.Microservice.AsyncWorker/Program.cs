using System.Text.Json.Serialization;
using CodeDesignPlus.Net.Mongo.Extensions;
using CodeDesignPlus.Net.Core.Extensions;
using CodeDesignPlus.Net.Redis.Extensions;
using CodeDesignPlus.Net.RabitMQ.Extensions;
using CodeDesignPlus.Net.PubSub.Extensions;
using CodeDesignPlus.Net.Observability.Extensions;
using CodeDesignPlus.Net.Logger.Extensions;
using CodeDesignPlus.Net.Security.Extensions;

var builder = WebApplication.CreateSlimBuilder(args);

Serilog.Debugging.SelfLog.Enable(Console.Error);

builder.Host.UseSerilog();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    //options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});


builder.Services.AddMongo(builder.Configuration);
builder.Services.AddCore(builder.Configuration);
builder.Services.AddRepositories<CodeDesignPlus.Net.Microservice.Infrastructure.Startup>();
builder.Services.AddPubSub(builder.Configuration);
builder.Services.AddRedis(builder.Configuration);
builder.Services.AddRabitMQ(builder.Configuration);
builder.Services.AddSecurity(builder.Configuration);

var app = builder.Build();

var todosApi = app.MapGroup("/home");

todosApi.MapGet("/", () => "Ready");

app.Run();