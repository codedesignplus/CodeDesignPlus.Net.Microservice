using CodeDesignPlus.Net.Core.Abstractions;
using CodeDesignPlus.Net.Core.Abstractions.Attributes;

namespace CodeDesignPlus.Net.Microservice.Domain.DomainEvents
{
    [Key("dtop.ms-archetype.v1.domain_event.product.removed")]
    public class ProductRemovedFromOrderDomainEvent(
        Guid aggregateId,
        Guid productId,
        Guid? eventId = null,
        DateTime? occurredAt = null,
        Dictionary<string, object>? metadata = null
    ) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
    {
        public Guid ProductId { get; } = productId;

        public static ProductRemovedFromOrderDomainEvent Create(Guid aggregateId, Guid productId)
        {
            return new ProductRemovedFromOrderDomainEvent(aggregateId, productId);
        }
    }
}
