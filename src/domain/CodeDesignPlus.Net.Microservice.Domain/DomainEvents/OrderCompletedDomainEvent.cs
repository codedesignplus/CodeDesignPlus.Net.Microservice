namespace CodeDesignPlus.Net.Microservice.Domain.DomainEvents;

[EventKey<OrderAggregate>(1, "OrderCompleted")]
public class OrderCompletedDomainEvent(
     Guid aggregateId,
     Instant completedAt,
     Guid? eventId = null,
     Instant? occurredAt = null,
     Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public Instant CompletedAt { get; } = completedAt;

    public static OrderCompletedDomainEvent Create(Guid aggregateId)
    {
        return new OrderCompletedDomainEvent(aggregateId, SystemClock.Instance.GetCurrentInstant());
    }
}
