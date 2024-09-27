namespace CodeDesignPlus.Net.Microservice.AsyncWorker.Test.Consumers;

public class CompleteOrderHandlerTest
{

    [Fact]
    public async Task HandleAsync_LogsInformation()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<CompleteOrderHandler>>();
        var handler = new CompleteOrderHandler(loggerMock.Object);
        var domainEvent = OrderCompletedDomainEvent.Create(Guid.NewGuid());

        var json = JsonConvert.SerializeObject(domainEvent);

        var cancellationToken = CancellationToken.None;

        // Act
        await handler.HandleAsync(domainEvent, cancellationToken);

        // Assert
        loggerMock.VerifyLogging($"OrderCompletedDomainEvent Recived, {domainEvent.AggregateId}, {json}", LogLevel.Information);
    }

}
