using CodeDesignPlus.Net.Core.Abstractions;
using CodeDesignPlus.Net.Core.Abstractions.Attributes;
using CodeDesignPlus.Net.Microservice.Domain.Entities;

namespace CodeDesignPlus.Net.Microservice.Domain.DomainEvents
{
    [Key("dtop.ms-archetype.v1.domain_event.product.added_to_order")]
    public class ProductAddedToOrderDomainEvent(
        Guid aggregateId,
        int quantity,
        ProductEntity product,
        Guid? eventId = null,
        DateTime? occurredAt = null,
        Dictionary<string, object>? metadata = null
    ) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
    {
        public int Quantity { get; } = quantity;
        public ProductEntity Product { get; set; } = product;

        public static ProductAddedToOrderDomainEvent Create(Guid aggregateId, int quantity, ProductEntity product)
        {
            return new ProductAddedToOrderDomainEvent(aggregateId, quantity, product);
        }
    }

}
