namespace CodeDesignPlus.Net.Microservice.AsyncWorker.Test.Consumers;

public class AddProductToOrderHandlerTest
{
    [Fact]
    public async Task HandleAsync_LogsInformation()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<AddProductToOrderHandler>>();
        var handler = new AddProductToOrderHandler(loggerMock.Object);
        var domainEvent = ProductAddedToOrderDomainEvent.Create(Guid.NewGuid(), 1, new ProductEntity()
        {
            Id = Guid.NewGuid(),
            Name = "Product Test",
            Price = 100
        });

        var json = JsonConvert.SerializeObject(domainEvent);

        var cancellationToken = CancellationToken.None;

        // Act
        await handler.HandleAsync(domainEvent, cancellationToken);

        // Assert
        loggerMock.VerifyLogging($"ProductAddedToOrderDomainEvent Recived, {domainEvent.AggregateId}, {json}", LogLevel.Information);
    }
}
