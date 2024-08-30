using System.Text.Json.Serialization;
using CodeDesignPlus.Net.Mongo.Extensions;
using CodeDesignPlus.Net.Core.Extensions;
using CodeDesignPlus.Net.Redis.Extensions;
using CodeDesignPlus.Net.RabbitMQ.Extensions;
using CodeDesignPlus.Net.PubSub.Extensions;
using CodeDesignPlus.Net.Observability.Extensions;
using CodeDesignPlus.Net.Logger.Extensions;
using CodeDesignPlus.Net.Security.Extensions;
using CodeDesignPlus.Net.Microservice.AsyncWorker;

var builder = WebApplication.CreateSlimBuilder(args);

Serilog.Debugging.SelfLog.Enable(Console.Error);

builder.Host.UseSerilog();

builder.Services.AddMongo<StartupServices>(builder.Configuration);
builder.Services.AddRedis(builder.Configuration);
builder.Services.AddRabbitMQ(builder.Configuration);
builder.Services.AddSecurity(builder.Configuration);

var app = builder.Build();

var home = app.MapGroup("/home");

home.MapGet("/", () => "Ready");

await app.RunAsync();