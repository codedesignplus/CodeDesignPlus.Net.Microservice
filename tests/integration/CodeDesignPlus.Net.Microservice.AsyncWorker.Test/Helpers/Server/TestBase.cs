using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Grpc.Net.Client;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.AsyncWorker.Test.Helpers.Server;

[Collection("Server Collection")]
public class TestBase(TestServer<Program> _server) : IAsyncLifetime
{
    private AsyncServiceScope _scope;
    protected IServiceProvider _services;
    protected HttpClient _client;

    protected InMemoryLoggerProvider _loggerProvider => _server.LoggerProvider;

    protected GrpcChannel _channel;

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    public Task InitializeAsync()
    {
        _client = _server.CreateClient();
        _scope = _server.Services.CreateAsyncScope();
        _services = _scope.ServiceProvider;

        // _channel = GrpcChannel.ForAddress("http://localhost:5001", new GrpcChannelOptions
        // {
        //     HttpClient = _client
        // });

        var options = new GrpcChannelOptions { HttpHandler = _server.Server.CreateHandler() };
        _channel = GrpcChannel.ForAddress(_server.Server.BaseAddress, options);

        return Task.CompletedTask;
    }
}