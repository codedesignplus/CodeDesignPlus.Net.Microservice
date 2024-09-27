namespace CodeDesignPlus.Net.Microservice.AsyncWorker.Test.Consumers;


[Collection("Server Collection")]
public class AddProductToOrderHandlerTest(TestServer<Program> server) : TestBase(server)
{

    [Fact]
    public async Task HandleAsync_Success()
    {
        // Arrange
        var messageService = this._services.GetRequiredService<IMessage>();

        var domainEvent = ProductAddedToOrderDomainEvent.Create(Guid.NewGuid(), 1, new Domain.Entities.ProductEntity()
        {
            Id = Guid.NewGuid(),
            Name = "Product 1"
        });

        await messageService.PublishAsync(domainEvent, CancellationToken.None);

        await Task.Delay(1000);

        // Act
        var logs = _loggerProvider.Loggers.SelectMany(x => x.Value.Logs).ToList();

        Assert.Contains(logs, log => log.Contains("ProductAddedToOrderDomainEvent Recived"));
        Assert.Contains(logs, log => log.Contains(domainEvent.AggregateId.ToString()));
        Assert.Contains(logs, log => log.Contains(JsonConvert.SerializeObject(domainEvent)));
    }
}
