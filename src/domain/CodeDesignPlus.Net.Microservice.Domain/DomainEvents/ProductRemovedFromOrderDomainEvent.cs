namespace CodeDesignPlus.Net.Microservice.Domain.DomainEvents;

[EventKey<ProductEntity>(1, "ProductRemovedFromOrder")]
public class ProductRemovedFromOrderDomainEvent(
    Guid aggregateId,
    Guid productId,
    Guid? eventId = null,
    Instant? occurredAt = null,
    Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public Guid ProductId { get; } = productId;

    public static ProductRemovedFromOrderDomainEvent Create(Guid aggregateId, Guid productId)
    {
        return new ProductRemovedFromOrderDomainEvent(aggregateId, productId);
    }
}
