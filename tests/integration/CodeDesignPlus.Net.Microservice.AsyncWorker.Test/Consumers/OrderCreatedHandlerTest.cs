namespace CodeDesignPlus.Net.Microservice.AsyncWorker.Test.Consumers;

[Collection("Server Collection")]
public class OrderCreatedHandlerTest(TestServer<Program> server) : TestBase(server)
{

    [Fact]
    public async Task HandleAsync_Success()
    {
        // Arrange
        var messageService = this._services.GetRequiredService<IMessage>();

        var domainEvent = OrderCreatedDomainEvent.Create(Guid.NewGuid(), new Domain.Entities.ClientEntity()
        {
            Id = Guid.NewGuid(),
            Name = "Client 1"
        }, Guid.NewGuid(), Guid.NewGuid());

        await messageService.PublishAsync(domainEvent, CancellationToken.None);

        await Task.Delay(1000);

        // Act
        var logs = _loggerProvider.Loggers.SelectMany(x => x.Value.Logs).ToList();

        Assert.Contains(logs, log => log.Contains("OrderCreatedDomainEvent Recived"));
        Assert.Contains(logs, log => log.Contains(domainEvent.AggregateId.ToString()));
        Assert.Contains(logs, log => log.Contains(JsonConvert.SerializeObject(domainEvent)));
    }
}
