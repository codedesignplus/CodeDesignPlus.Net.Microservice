using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeDesignPlus.Net.Microservice.gRpc.Test.Helpers.Server;

public class TestServer<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    public ServerCompose Compose { get; }

    public TestServer()
    {
        Compose = new ServerCompose();

        Thread.Sleep(5000);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {

        // Set the environment variable for the service.

        builder.ConfigureAppConfiguration(x =>
        {

            x.AddInMemoryCollection(new Dictionary<string, string?>()
            {
                {"Redis:Instances:Core:ConnectionString", $"{Compose.Redis.Item1}:{Compose.Redis.Item2}"},
                {"RabbitMQ:Host", Compose.RabbitMQ.Item1},
                {"RabbitMQ:Port", Compose.RabbitMQ.Item2.ToString()},
                {"MongoDB:ConnectionString", $"mongodb://{Compose.Mongo.Item1}:{Compose.Mongo.Item2}"},
                {"Observability:ServerOtel", $"http://{Compose.Otel.Item1}:{Compose.Otel.Item2}"},
                {"Logger:OTelEndpoint", $"http://{Compose.Otel.Item1}:{Compose.Otel.Item2}" },
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
        Compose.StopInstance();

        base.Dispose(disposing);
    }
}