namespace CodeDesignPlus.Net.Microservice.AsyncWorker.Test.Consumers;

public class ProductRemovedFromOrderHandlerTest
{

    [Fact]
    public async Task HandleAsync_LogsInformation()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<ProductRemovedFromOrderHandler>>();
        var handler = new ProductRemovedFromOrderHandler(loggerMock.Object);
        var domainEvent = ProductRemovedFromOrderDomainEvent.Create(Guid.NewGuid(), Guid.NewGuid());

        var json = JsonConvert.SerializeObject(domainEvent);

        var cancellationToken = CancellationToken.None;

        // Act
        await handler.HandleAsync(domainEvent, cancellationToken);

        // Assert
        loggerMock.VerifyLogging($"ProductRemovedFromOrderDomainEvent Recived, {domainEvent.AggregateId}, {json}", LogLevel.Information);
    }
}
