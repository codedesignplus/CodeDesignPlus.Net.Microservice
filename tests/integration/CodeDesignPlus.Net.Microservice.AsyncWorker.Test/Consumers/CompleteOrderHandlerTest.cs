namespace CodeDesignPlus.Net.Microservice.AsyncWorker.Test.Consumers;

[Collection("Server Collection")]
public class CompleteOrderHandlerTest(TestServer<Program> server) : TestBase(server)
{

    [Fact]
    public async Task HandleAsync_Success()
    {
        // Arrange
        var messageService = this._services.GetRequiredService<IMessage>();

        var domainEvent = OrderCompletedDomainEvent.Create(Guid.NewGuid());

        await messageService.PublishAsync(domainEvent, CancellationToken.None);

        await Task.Delay(1000);

        // Act
        var logs = _loggerProvider.Loggers.SelectMany(x => x.Value.Logs).ToList();

        Assert.Contains(logs, log => log.Contains("OrderCompletedDomainEvent Recived"));
        Assert.Contains(logs, log => log.Contains(domainEvent.AggregateId.ToString()));
        Assert.Contains(logs, log => log.Contains(JsonConvert.SerializeObject(domainEvent)));
    }
}
