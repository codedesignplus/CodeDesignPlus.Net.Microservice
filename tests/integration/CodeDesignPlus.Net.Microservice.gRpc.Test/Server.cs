using CodeDesignPlus.Net.Microservice.gRpc.Test.Helpers;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace CodeDesignPlus.Net.Microservice.gRpc.Test;

public class Server : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> factory;

    public Server(CustomWebApplicationFactory<Program> customWebApplicationFactory)
    {
        factory = customWebApplicationFactory;
    }

    [Fact]
    public void Test1()
    {

    }
}