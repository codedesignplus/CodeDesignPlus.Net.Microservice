using Ductus.FluentDocker.Model.Compose;
using Ductus.FluentDocker.Services;
using Ductus.FluentDocker.Services.Extensions;
using Ductus.FluentDocker.Services.Impl;
using Xunit.Sdk;

namespace CodeDesignPlus.Net.Microservice.AsyncWorker.Test.Helpers.Server;

public class ServerCompose : DockerCompose
{
    private readonly string containerName = "server_" + Guid.NewGuid().ToString("N");

    protected override ICompositeService Build()
    {
        // Define the path to the Docker Compose file.
        var file = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "Server", "docker-compose.yml");

        // Configure the Docker Compose settings.
        var dockerCompose = new DockerComposeConfig
        {
            ComposeFilePath = [file],
            ForceRecreate = true,
            RemoveOrphans = true,
            StopOnDispose = true,
            AlternativeServiceName = this.containerName,
        };

        // Create and return the Docker Compose service.
        var compose = new DockerComposeCompositeService(base.DockerHost, dockerCompose);

        return compose;
    }

    /// <summary>
    /// Called when the Docker container is initialized.
    /// </summary>
    protected override void OnContainerInitialized()
    {
        // Get the port for the service.
        var (hostRedis, portRedis) = this.GetPort("redis", 6379);
        var (hostRabbitMQ, portRabbitMQ) = this.GetPort("rabbitmq", 5672);
        var (hostMongo, portMongo) = this.GetPort("mongo", 27017);
        var (hostOtel, portOtel) = this.GetPort("otel-collector", 4317);

        // Set the environment variable for the service.
        Environment.SetEnvironmentVariable("Redis.Instances.Core.ConnectionString", $"{hostRedis}:{portRedis}", EnvironmentVariableTarget.Process);

        Environment.SetEnvironmentVariable("RabbitMQ.Host", hostRabbitMQ, EnvironmentVariableTarget.Process);
        Environment.SetEnvironmentVariable("RabbitMQ.Port", portRabbitMQ.ToString(), EnvironmentVariableTarget.Process);

        Environment.SetEnvironmentVariable("MongoDB.ConnectionString", $"mongodb://{hostMongo}:{portMongo}", EnvironmentVariableTarget.Process);

        Environment.SetEnvironmentVariable("Observability.ServerOtel", $"http://{hostOtel}:{portOtel}", EnvironmentVariableTarget.Process);
        Environment.SetEnvironmentVariable("Logger.OTelEndpoint", $"http://{hostOtel}:{portOtel}", EnvironmentVariableTarget.Process);

    }

    /// <summary>
    /// Get the host and port for the service.
    /// </summary>
    /// <param name="service">The name of the service.</param>
    /// <param name="InternalPort">The internal port of the service.</param>
    /// <returns>A tuple containing the host and port for the service.</returns>
    /// <exception cref="XunitException">The container was not found.</exception>
    public (string, int) GetPort(string service, int InternalPort)
    {
        var name = $"{this.containerName}-{service}";
        var container = this.CompositeService.Containers.FirstOrDefault(x => x.Name.StartsWith(name));

        if (container == null)
            throw new XunitException($"The container {name} was not found.");

        var endpoint = container.ToHostExposedEndpoint($"{InternalPort}/tcp");

        return (endpoint.Address.ToString(), endpoint.Port);
    }

}
