using System;

namespace CodeDesignPlus.Net.Microservice.AsyncWorker.Test.Consumers;

public class UpdateQuantityProductHandlerTest
{

    [Fact]
    public async Task HandleAsync_LogsInformation()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<UpdateQuantityProductHandler>>();
        var handler = new UpdateQuantityProductHandler(loggerMock.Object);
        var domainEvent = ProductQuantityUpdatedDomainEvent.Create(Guid.NewGuid(), Guid.NewGuid(), 10);

        var json = JsonConvert.SerializeObject(domainEvent);

        var cancellationToken = CancellationToken.None;

        // Act
        await handler.HandleAsync(domainEvent, cancellationToken);

        // Assert
        loggerMock.VerifyLogging($"ProductQuantityUpdatedDomainEvent Recived, {domainEvent.AggregateId}, {json}", LogLevel.Information);
    }

}
