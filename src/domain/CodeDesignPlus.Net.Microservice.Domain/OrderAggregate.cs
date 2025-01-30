using CodeDesignPlus.Net.Microservice.Domain.ValueObjects;

namespace CodeDesignPlus.Net.Microservice.Domain;

public class OrderAggregate(Guid id) : AggregateRoot(id)
{
    public Instant? CompletedAt { get; private set; }
    public Instant? CancelledAt { get; private set; }
    public ClientValueObject Client { get; private set; } = default!;
    public List<ProductEntity> Products { get; private set; } = [];
    public OrderStatus Status { get; private set; }
    public string? ReasonForCancellation { get; private set; }
    public AddressValueObject ShippingAddress { get; private set; } = default!;

    public static OrderAggregate Create(Guid id, ClientValueObject client, AddressValueObject shippingAddress, Guid tenant, Guid createdBy)
    {
        DomainGuard.GuidIsEmpty(id, Errors.IdOrderIsInvalid);
        DomainGuard.IsNull(client, Errors.ClientIsNull);
        DomainGuard.GuidIsEmpty(tenant, Errors.TenantIsInvalid);
        DomainGuard.IsNull(shippingAddress, Errors.AddressIsNull);

        var aggregate = new OrderAggregate(id)
        {
            Client = client,
            ShippingAddress = shippingAddress,
            CreatedAt = SystemClock.Instance.GetCurrentInstant(),
            Status = OrderStatus.Created,
            Tenant = tenant,
            CreatedBy = createdBy
        };

        aggregate.AddEvent(OrderCreatedDomainEvent.Create(aggregate.Id, aggregate.Client, aggregate.ShippingAddress, aggregate.Tenant, aggregate.CreatedBy));

        return aggregate;
    }

    public void AddProduct(Guid id, string name, string description, long price, int quantity, Guid updateBy)
    {
        DomainGuard.GuidIsEmpty(id, Errors.IdProductIsInvalid);
        DomainGuard.IsNullOrEmpty(name, Errors.NameProductIsInvalid);
        DomainGuard.IsLessThan(price, 0, Errors.PriceProductIsInvalid);
        DomainGuard.IsLessThan(quantity, 0, Errors.QuantityProductIsInvalid);

        this.UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        this.UpdatedBy = updateBy;

        var product = new ProductEntity
        {
            Id = id,
            Name = name,
            Description = description,
            Price = price,
            Quantity = quantity
        };

        Products.Add(product);

        AddEvent(ProductAddedToOrderDomainEvent.Create(Id, quantity, product));
    }

    public void RemoveProduct(Guid productId, Guid updateBy)
    {
        DomainGuard.GuidIsEmpty(productId, Errors.IdProductIsInvalid);

        var product = Products.SingleOrDefault(x => x.Id == productId);

        DomainGuard.IsNull(product, Errors.ProductNotFound);

        this.UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        this.UpdatedBy = updateBy;

        Products.Remove(product);

        AddEvent(ProductRemovedFromOrderDomainEvent.Create(Id, productId));
    }

    public void UpdateProductQuantity(Guid productId, int newQuantity, Guid updateBy)
    {
        DomainGuard.GuidIsEmpty(productId, Errors.IdProductIsInvalid);
        DomainGuard.IsLessThan(newQuantity, 0, Errors.QuantityProductIsInvalid);

        var product = Products.SingleOrDefault(p => p.Id == productId);

        DomainGuard.IsNull(product, Errors.ProductNotFound);

        this.UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        this.UpdatedBy = updateBy;

        product.Quantity = newQuantity;

        AddEvent(ProductQuantityUpdatedDomainEvent.Create(Id, productId, newQuantity));
    }

    public void CompleteOrder(Guid updateBy)
    {
        DomainGuard.IsTrue(Status == OrderStatus.Cancelled, Errors.OrderAlreadyCancelled);
        DomainGuard.IsTrue(Status == OrderStatus.Completed, Errors.OrderAlreadyCompleted);

        var @event = OrderCompletedDomainEvent.Create(Id);

        this.UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        this.UpdatedBy = updateBy;

        this.CompletedAt = @event.CompletedAt;
        this.Status = OrderStatus.Completed;

        AddEvent(OrderCompletedDomainEvent.Create(Id));
    }

    public void CancelOrder(string reason, Guid updateBy)
    {
        DomainGuard.IsTrue(Status == OrderStatus.Cancelled, Errors.OrderAlreadyCancelled);

        this.UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        this.UpdatedBy = updateBy;
        this.ReasonForCancellation = reason;
        this.CancelledAt = SystemClock.Instance.GetCurrentInstant();
        this.Status = OrderStatus.Cancelled;

        AddEvent(OrderCancelledDomainEvent.Create(Id, reason));
    }
}
