using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace CodeDesignPlus.Net.Microservice.Rest.Test.Helpers.Server;


[Collection("Server Collection")]
public class TestBase(TestServer<Program> _server) : IAsyncLifetime
{
    private AsyncServiceScope _scope;
    protected IServiceProvider _services;
    protected HttpClient _client;

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    public Task InitializeAsync()
    {
        _client = _server.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost/")
        });
        _scope = _server.Services.CreateAsyncScope();
        _services = _scope.ServiceProvider;

        return Task.CompletedTask;
    }
}