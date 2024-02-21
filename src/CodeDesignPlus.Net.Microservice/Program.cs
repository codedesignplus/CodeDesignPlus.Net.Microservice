using CodeDesignPlus.Net.Core.Extensions;
using CodeDesignPlus.Net.Kafka.Extensions;
using CodeDesignPlus.Net.Mongo.Extensions;
using CodeDesignPlus.Net.PubSub.Extensions;
using CodeDesignPlus.Net.Redis.Extensions;
using CodeDesignPlus.Net.Redis.PubSub.Extensions;
using CodeDesignPlus.Net.Event.Sourcing.Extensions;
using CodeDesignPlus.Net.EventStore.Extensions;
using CodeDesignPlus.Net.EventStore.PubSub.Extensions;
using Mapster;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMapster();
builder.Services.AddMediatR(x =>
{
    x.RegisterServicesFromAssembly(typeof(CodeDesignPlus.Net.Microservice.Application.Startup).Assembly);
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
