using CodeDesignPlus.Net.Core.Abstractions;
using CodeDesignPlus.Net.Core.Abstractions.Attributes;

namespace CodeDesignPlus.Net.Microservice.Domain.DomainEvents
{
    [Key("dtop.ms-archetype.v1.domain_event.order.name_updated")]
    public class OrderCompletedDomainEvent(
         Guid aggregateId,
         DateTime completionDate,
         Guid? eventId = null,
         DateTime? occurredAt = null,
         Dictionary<string, object>? metadata = null
    ) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
    {
        public DateTime CompletionDate { get; } = completionDate;

        public static OrderCompletedDomainEvent Create(Guid aggregateId)
        {
            return new OrderCompletedDomainEvent(aggregateId, DateTime.UtcNow);
        }
    }
}
