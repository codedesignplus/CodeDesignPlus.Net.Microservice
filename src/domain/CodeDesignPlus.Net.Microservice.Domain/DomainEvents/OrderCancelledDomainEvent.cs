

namespace CodeDesignPlus.Net.Microservice.Domain.DomainEvents;

[Key("dtop.ms-archetype.v1.domain_event.order.cancelled")]
public class OrderCancelledDomainEvent(
    Guid aggregateId,
    long cancelledAt,
    string reason,
    Guid? eventId = null,
    DateTime? occurredAt = null,
    Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public long CancelledAt { get; } = cancelledAt;
    public string Reason { get; } = reason;

    public static OrderCancelledDomainEvent Create(Guid aggregateId, string reason)
    {
        return new OrderCancelledDomainEvent(aggregateId, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), reason);
    }
}

