namespace CodeDesignPlus.Net.Microservice.Domain.DomainEvents;

[EventKey<OrderAggregate>(1, "OrderCancelled")]
public class OrderCancelledDomainEvent(
    Guid aggregateId,
    Instant cancelledAt,
    string reason,
    Guid? eventId = null,
    Instant? occurredAt = null,
    Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public Instant CancelledAt { get; } = cancelledAt;
    public string Reason { get; } = reason;

    public static OrderCancelledDomainEvent Create(Guid aggregateId, string reason)
    {
        return new OrderCancelledDomainEvent(aggregateId, SystemClock.Instance.GetCurrentInstant(), reason);
    }
}

