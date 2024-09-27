using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CodeDesignPlus.Net.Microservice.AsyncWorker.Test.Helpers.Server;

public class TestServer<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    public ServerCompose Compose { get; }

    public TestServer()
    {
        this.Compose = new ServerCompose();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {

        });

        builder.UseEnvironment("Development");
    }

    protected override void Dispose(bool disposing)
    {
        this.Compose.StopInstance();

        base.Dispose(disposing);
    }
}