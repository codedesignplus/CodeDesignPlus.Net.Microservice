using Ductus.FluentDocker.Model.Compose;
using Ductus.FluentDocker.Services;
using Ductus.FluentDocker.Services.Extensions;
using Ductus.FluentDocker.Services.Impl;
using Xunit.Sdk;

namespace CodeDesignPlus.Net.Microservice.Rest.Test.Helpers.Server;

public class ServerCompose : DockerCompose
{
    private readonly string containerName = "server_" + Guid.NewGuid().ToString("N");

    public (string, int) Redis;
    public (string, int) RabbitMQ;
    public (string, int) Mongo;
    public (string, int) Otel;

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
        this.Redis = this.GetPort("redis", 6379);
        this.RabbitMQ = this.GetPort("rabbitmq", 5672);
        this.Mongo = this.GetPort("mongo", 27017);
        this.Otel = this.GetPort("otel-collector", 4317);

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

        return ("localhost", endpoint.Port);
    }

}
