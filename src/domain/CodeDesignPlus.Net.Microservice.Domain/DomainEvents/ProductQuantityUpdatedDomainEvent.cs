

namespace CodeDesignPlus.Net.Microservice.Domain.DomainEvents;

[Key("dtop.ms-archetype.v1.domain_event.product.quantity_updated")]
public class ProductQuantityUpdatedDomainEvent(
    Guid aggregateId,
    Guid productId,
    int newQuantity,
    Guid? eventId = null,
    DateTime? occurredAt = null,
    Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public Guid ProductId { get; } = productId;
    public int NewQuantity { get; } = newQuantity;

    public static ProductQuantityUpdatedDomainEvent Create(Guid aggregateId, Guid productId, int newQuantity)
    {
        return new ProductQuantityUpdatedDomainEvent(aggregateId, productId, newQuantity);
    }
}
