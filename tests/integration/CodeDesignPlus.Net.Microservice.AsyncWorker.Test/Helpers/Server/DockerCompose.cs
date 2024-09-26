using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ductus.FluentDocker.Services;

namespace CodeDesignPlus.Net.Microservice.AsyncWorker.Test.Helpers.Server;

public abstract class DockerCompose
{

    /// <summary>
    /// Gets or sets the composite service for Docker Compose.
    /// </summary>
    protected ICompositeService CompositeService;

    /// <summary>
    /// Gets or sets the Docker host service.
    /// </summary>
    protected IHostService? DockerHost;

    /// <summary>
    /// Initializes a new instance of the <see cref="DockerCompose"/> class.
    /// </summary>
    protected DockerCompose()
    {
        this.EnsureDockerHost();

        this.CompositeService = this.Build();

        try
        {
            this.CompositeService.Start();
        }
        catch
        {
            this.CompositeService.Dispose();

            throw;
        }

        this.OnContainerInitialized();
    }

    
    /// <summary>
    /// Builds the Docker Compose service configuration.
    /// </summary>
    /// <returns>An <see cref="ICompositeService"/> representing the Docker Compose service.</returns>
    protected abstract ICompositeService Build();


    /// <summary>
    /// Called when the Docker container is being torn down.
    /// </summary>
    protected virtual void OnContainerTearDown()
    {
    }

    /// <summary>
    /// Called when the Docker container is initialized.
    /// </summary>
    protected virtual void OnContainerInitialized()
    {
    }

    /// <summary>
    /// Ensures that the Docker host is running.
    /// </summary>
    private void EnsureDockerHost()
    {
        if (this.DockerHost?.State == ServiceRunningState.Running)
            return;

        var hosts = new Hosts().Discover();
        this.DockerHost = hosts.FirstOrDefault(x => x.IsNative) ?? hosts.FirstOrDefault(x => x.Name == "default");

        if (this.DockerHost != null)
        {
            if (this.DockerHost.State != ServiceRunningState.Running)
                this.DockerHost.Start();

            return;
        }

        if (hosts.Count > 0)
            this.DockerHost = hosts[0];

        if (this.DockerHost == null)
            this.EnsureDockerHost();
    }

    /// <summary>
    /// Stops the Docker container instance.
    /// </summary>
    public void StopInstance()
    {
        this.OnContainerTearDown();
        var compositeService = this.CompositeService;
        this.CompositeService = null!;
        try
        {
            compositeService?.Dispose();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}

