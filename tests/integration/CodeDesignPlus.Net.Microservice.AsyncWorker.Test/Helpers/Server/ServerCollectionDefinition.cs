using Xunit;
namespace CodeDesignPlus.Net.Microservice.AsyncWorker.Test.Helpers.Server;

[CollectionDefinition("Server Collection")]
public class ServerCollectionDefinition : ICollectionFixture<TestServer<Program>>
{
}