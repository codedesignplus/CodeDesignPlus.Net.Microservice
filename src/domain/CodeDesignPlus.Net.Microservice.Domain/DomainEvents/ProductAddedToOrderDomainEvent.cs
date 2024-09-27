namespace CodeDesignPlus.Net.Microservice.Domain.DomainEvents;

[EventKey<ProductEntity>(1, "ProductAddedToOrder")]
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
