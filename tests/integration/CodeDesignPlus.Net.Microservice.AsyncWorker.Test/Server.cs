using CodeDesignPlus.Net.Microservice.AsyncWorker.Test.Helpers.Server;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.AsyncWorker.Test;

[Collection("Server Collection")]
public class Server(TestServer<Program> server) : TestBase(server)
{


    [Fact]
    public void Test1()
    {
        var httpClient = _client;
    }
}