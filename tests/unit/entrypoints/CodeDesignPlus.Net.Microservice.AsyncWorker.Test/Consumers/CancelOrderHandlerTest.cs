namespace CodeDesignPlus.Net.Microservice.AsyncWorker.Test.Consumers;

public class CancelOrderHandlerTest
{
    [Fact]
    public async Task HandleAsync_LogsInformation()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<CancelOrderHandler>>();
        var handler = new CancelOrderHandler(loggerMock.Object);
        var domainEvent = OrderCancelledDomainEvent.Create(Guid.NewGuid(), "The order was canceled");

        var json = JsonConvert.SerializeObject(domainEvent);

        var cancellationToken = CancellationToken.None;

        // Act
        await handler.HandleAsync(domainEvent, cancellationToken);

        // Assert
        loggerMock.VerifyLogging($"OrderCancelledDomainEvent Recived, {domainEvent.AggregateId}, {json}", LogLevel.Information);
    }

}
