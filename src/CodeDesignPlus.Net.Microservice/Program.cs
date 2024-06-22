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

var builder = WebApplication.CreateSlimBuilder(args);

Serilog.Debugging.SelfLog.Enable(Console.Error);

builder.Host.UseSerilog();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
var info = new OpenApiInfo()
{
    Title = "Orders",
    Version = "v1",
    Description = "Microservice Template",
    Contact = new OpenApiContact()
    {
        Name = "CodeDesignPlus",
        Email = "codedesignplus@outlook.com",
    }
};

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", info);

    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

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


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseErrorHandler();

app.UseObservability();

app.UseHttpsRedirection();

app.UseAuth();

app.MapControllers();


app.Run();
