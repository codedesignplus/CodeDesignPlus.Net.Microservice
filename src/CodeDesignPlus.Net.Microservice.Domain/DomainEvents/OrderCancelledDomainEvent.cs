using CodeDesignPlus.Net.Core.Abstractions;
using CodeDesignPlus.Net.Core.Abstractions.Attributes;

namespace CodeDesignPlus.Net.Microservice.Domain.DomainEvents
{
    [Key("dtop.ms-archetype.v1.domain_event.order.cancelled")]
    public class OrderCancelledDomainEvent(
        Guid aggregateId,
        DateTime cancellationDate,
        string reason,
        Guid? eventId = null,
        DateTime? occurredAt = null,
        Dictionary<string, object>? metadata = null
    ) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
    {
        public DateTime CancellationDate { get; } = cancellationDate;
        public string Reason { get; } = reason;

        public static OrderCancelledDomainEvent Create(Guid aggregateId, string reason)
        {
            return new OrderCancelledDomainEvent(aggregateId, DateTime.UtcNow, reason);
        }
    }
}
