namespace CodeDesignPlus.Net.Microservice.AsyncWorker.Test.Consumers;

[Collection("Server Collection")]
public class CancelOrderHandlerTest(TestServer<Program> server) : TestBase(server)
{

    [Fact]
    public async Task HandleAsync_Success()
    {
        // Arrange
        var messageService = this._services.GetRequiredService<IMessage>();

        var domainEvent = OrderCancelledDomainEvent.Create(Guid.NewGuid(), "Custom Cancellation");

        await messageService.PublishAsync(domainEvent, CancellationToken.None);

        await Task.Delay(1000);

        // Act
        var logs = _loggerProvider.Loggers.SelectMany(x => x.Value.Logs).ToList();

        Assert.Contains(logs, log => log.Contains("OrderCancelledDomainEvent Recived"));
        Assert.Contains(logs, log => log.Contains(domainEvent.AggregateId.ToString()));
        Assert.Contains(logs, log => log.Contains(JsonConvert.SerializeObject(domainEvent)));
    }
}
