namespace CodeDesignPlus.Net.Microservice.AsyncWorker.Test.Consumers;

public class OrderCreatedHandlerTest(Server<Program> server) : ServerBase<Program>(server), IClassFixture<Server<Program>>
{

    [Fact]
    public async Task HandleAsync_Success()
    {
        await Task.Delay(5000);
        
        // Arrange
        var messageService = Services.GetRequiredService<IMessage>();

        var domainEvent = OrderCreatedDomainEvent.Create(Guid.NewGuid(), new Domain.Entities.ClientEntity()
        {
            Id = Guid.NewGuid(),
            Name = "Client 1"
        }, Guid.NewGuid(), Guid.NewGuid());

        await messageService.PublishAsync(domainEvent, CancellationToken.None);

        await Task.Delay(1000);

        // Act
        var logs = LoggerProvider.Loggers.SelectMany(x => x.Value.Logs).ToList();

        Assert.Contains(logs, log => log.Contains("OrderCreatedDomainEvent Recived"));
        Assert.Contains(logs, log => log.Contains(domainEvent.AggregateId.ToString()));
        Assert.Contains(logs, log => log.Contains(JsonConvert.SerializeObject(domainEvent)));
    }
}
