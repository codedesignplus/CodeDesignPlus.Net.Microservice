using CodeDesignPlus.Net.Microservice.Commons.EntryPoints.Rest.Middlewares;
using CodeDesignPlus.Net.Microservice.Commons.EntryPoints.Rest.Swagger;
using CodeDesignPlus.Net.Microservice.Commons.FluentValidation;
using CodeDesignPlus.Net.Microservice.Commons.MediatR;

var builder = WebApplication.CreateSlimBuilder(args);

Serilog.Debugging.SelfLog.Enable(Console.Error);

builder.Host.UseSerilog();


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddRedis(builder.Configuration);
builder.Services.AddMongo<CodeDesignPlus.Net.Microservice.Infrastructure.Startup>(builder.Configuration);
builder.Services.AddObservability(builder.Configuration, builder.Environment, x => { }, x => { });
builder.Services.AddLogger(builder.Configuration);
builder.Services.AddRabbitMQ(builder.Configuration);
builder.Services.AddMapster();
builder.Services.AddFluentValidation();
builder.Services.AddMediatR<CodeDesignPlus.Net.Microservice.Application.Startup>();
builder.Services.AddSecurity(builder.Configuration);
builder.Services.AddCoreSwagger<Program>(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCoreSwagger();

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuth();

app.MapControllers().RequireAuthorization();

await app.RunAsync();

public partial class Program { }
