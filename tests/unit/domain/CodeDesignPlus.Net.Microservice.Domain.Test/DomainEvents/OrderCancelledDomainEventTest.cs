namespace CodeDesignPlus.Net.Microservice.Domain.Test.DomainEvents;

public class OrderCancelledDomainEventTest
{

    [Fact]
    public void Create_DefaultValues_Success()
    {
        // Arrange
        var aggregateId = Guid.NewGuid();
        var reason = "Some reason";

        // Act
        var domainEvent = OrderCancelledDomainEvent.Create(aggregateId, reason);

        // Assert
        Assert.NotNull(domainEvent);
        Assert.Equal(aggregateId, domainEvent.AggregateId);
        Assert.Equal(reason, domainEvent.Reason);
        Assert.True(domainEvent.CancelledAt > 0);
    }

    [Fact]
    public void Create_CustomValues_Success()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var aggregateId = Guid.NewGuid();
        var reason = "Some reason";
        var cancelledAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var occurredAt = DateTime.UtcNow;
        var metadata = new Dictionary<string, object>();

        // Act
        var domainEvent = new OrderCancelledDomainEvent(aggregateId, cancelledAt, reason, eventId, occurredAt, metadata);

        // Assert
        Assert.NotNull(domainEvent);
        Assert.Equal(aggregateId, domainEvent.AggregateId);
        Assert.Equal(reason, domainEvent.Reason);
        Assert.Equal(cancelledAt, domainEvent.CancelledAt);
        Assert.Equal(eventId, domainEvent.EventId);
        Assert.Equal(occurredAt, domainEvent.OccurredAt);
        Assert.Equal(metadata, domainEvent.Metadata);
    }
}
