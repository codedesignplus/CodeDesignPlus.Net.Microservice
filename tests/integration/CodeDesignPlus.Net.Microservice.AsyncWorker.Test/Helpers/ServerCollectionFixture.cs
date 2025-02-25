using System;

namespace CodeDesignPlus.Net.Microservice.AsyncWorker.Test.Helpers;

public sealed class ServerCollectionFixture<TProgram> : IDisposable where TProgram : class
{
    /// <summary>
    /// The name of the collection.
    /// </summary>
    public const string Collection = "Server Collection";

    /// <summary>
    /// Gets the MongoDB container instance.
    /// </summary>
    public Server<TProgram> Container { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ServerCollectionFixture{TProgram}"/> class.
    /// </summary>
    public ServerCollectionFixture()
    {
        this.Container = new Server<TProgram>();
    }

    /// <summary>
    /// Disposes the MongoDB container instance.
    /// </summary>
    public void Dispose()
    {
        this.Container.Dispose();
    }
}