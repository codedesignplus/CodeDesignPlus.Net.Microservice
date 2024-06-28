

namespace CodeDesignPlus.Net.Microservice.Domain.DomainEvents;

[Key("dtop.ms-archetype.v1.domain_event.order.name_updated")]
public class OrderCompletedDomainEvent(
     Guid aggregateId,
     long completedAt,
     Guid? eventId = null,
     DateTime? occurredAt = null,
     Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public long CompletedAt { get; } = completedAt;

    public static OrderCompletedDomainEvent Create(Guid aggregateId)
    {
        return new OrderCompletedDomainEvent(aggregateId, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
    }
}
