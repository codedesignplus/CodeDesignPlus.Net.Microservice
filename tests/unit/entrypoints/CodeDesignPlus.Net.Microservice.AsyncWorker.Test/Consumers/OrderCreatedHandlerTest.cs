using System;

namespace CodeDesignPlus.Net.Microservice.AsyncWorker.Test.Consumers;

public class OrderCreatedHandlerTest
{

    [Fact]
    public async Task HandleAsync_LogsInformation()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<OrderCreatedHandler>>();
        var handler = new OrderCreatedHandler(loggerMock.Object);
        var domainEvent = OrderCreatedDomainEvent.Create(Guid.NewGuid(), new ClientEntity()
        {
            Id = Guid.NewGuid(),
            Name = "Client Name"
        }, Guid.NewGuid(), Guid.NewGuid());

        var json = JsonConvert.SerializeObject(domainEvent);

        var cancellationToken = CancellationToken.None;

        // Act
        await handler.HandleAsync(domainEvent, cancellationToken);

        // Assert
        loggerMock.VerifyLogging($"OrderCreatedDomainEvent Recived, {domainEvent.AggregateId}, {json}", LogLevel.Information);
    }

}
