using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeDesignPlus.Net.Microservice.Rest.Test.Helpers.Server;

public class TestServer<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    public ServerCompose Compose { get; }

    public TestServer()
    {
        this.Compose = new ServerCompose();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {

        // Set the environment variable for the service.

        builder.ConfigureAppConfiguration(x =>
        {

            x.AddInMemoryCollection(new Dictionary<string, string?>()
            {
                {"Redis:Instances:Core:ConnectionString", $"{this.Compose.Redis.Item1}:{this.Compose.Redis.Item2}"},
                {"RabbitMQ:Host", this.Compose.RabbitMQ.Item1},
                {"RabbitMQ:Port", this.Compose.RabbitMQ.Item2.ToString()},
                {"MongoDB:ConnectionString", $"mongodb://{this.Compose.Mongo.Item1}:{this.Compose.Mongo.Item2}"},
                {"Observability:ServerOtel", $"http://{this.Compose.Otel.Item1}:{this.Compose.Otel.Item2}"},
                {"Logger:OTelEndpoint", $"http://{this.Compose.Otel.Item1}:{this.Compose.Otel.Item2}" },
            });
        });

        builder.ConfigureServices(services =>
        {
            services.AddAuthentication("TestAuth")
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestAuth", options => { });
        });

        builder.UseEnvironment("Development");
    }

    protected override void Dispose(bool disposing)
    {
        this.Compose.StopInstance();

        base.Dispose(disposing);
    }
}