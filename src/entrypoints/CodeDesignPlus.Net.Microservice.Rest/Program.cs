using CodeDesignPlus.Net.Core.Extensions;
using CodeDesignPlus.Net.Kafka.Extensions;
using CodeDesignPlus.Net.Mongo.Extensions;
using CodeDesignPlus.Net.PubSub.Extensions;
using CodeDesignPlus.Net.Redis.Extensions;
using CodeDesignPlus.Net.Redis.PubSub.Extensions;
using CodeDesignPlus.Net.Event.Sourcing.Extensions;
using CodeDesignPlus.Net.EventStore.Extensions;
using CodeDesignPlus.Net.EventStore.PubSub.Extensions;
using CodeDesignPlus.Net.Logger.Extensions;
using CodeDesignPlus.Net.Observability.Extensions;
using CodeDesignPlus.Net.RabitMQ.Extensions;
using CodeDesignPlus.Net.Security.Extensions;
using CodeDesignPlus.Net.Exceptions.Extensions;
using Microsoft.OpenApi.Models;
using System.Reflection;
using CodeDesignPlus.Net.Microservice.Rest.Core.FluentValidation;
using CodeDesignPlus.Net.Microservice.Rest.Core.MediatR;
using CodeDesignPlus.Net.Microservice.Rest.Core.Middlewares;
using CodeDesignPlus.Net.Microservice.Rest.Core.Swagger;

var builder = WebApplication.CreateSlimBuilder(args);

Serilog.Debugging.SelfLog.Enable(Console.Error);

builder.Host.UseSerilog();


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCore(builder.Configuration);
builder.Services.AddRepositories<CodeDesignPlus.Net.Microservice.Infrastructure.Startup>();
builder.Services.AddPubSub(builder.Configuration);
builder.Services.AddRedis(builder.Configuration);
builder.Services.AddRedisPubSub(builder.Configuration);
builder.Services.AddKafka(builder.Configuration);
builder.Services.AddEventSourcing(builder.Configuration);
builder.Services.AddEventStore(builder.Configuration);
builder.Services.AddEventStorePubSub(builder.Configuration);
builder.Services.AddMongo(builder.Configuration);
builder.Services.AddObservability(builder.Configuration);
builder.Services.AddLogger(builder.Configuration);
builder.Services.AddRabitMQ(builder.Configuration);
builder.Services.AddMapster();
builder.Services.AddFluentValidation();
builder.Services.AddMediatRR();
builder.Services.AddSecurity(builder.Configuration);
builder.Services.AddCoreSwagger(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCoreSwagger();

app.UseMiddleware<ExceptionMiddlware>();


//app.UseObservability();

app.UseHttpsRedirection();

app.UseAuth();

app.MapControllers();


app.Run();
