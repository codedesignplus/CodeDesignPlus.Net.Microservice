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
builder.Services.AddMediatRR();
builder.Services.AddSecurity(builder.Configuration);
builder.Services.AddCoreSwagger(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCoreSwagger();

app.UseMiddleware<ExceptionMiddlware>();

app.UseHttpsRedirection();

app.UseAuth();

app.MapControllers().RequireAuthorization();

await app.RunAsync();

public partial class Program { }
