using CodeDesignPlus.Net.Core.Abstractions;
using CodeDesignPlus.Net.Exceptions;
using CodeDesignPlus.Net.Microservice.Domain.DomainEvents;
using CodeDesignPlus.Net.Microservice.Domain.Entities;
using CodeDesignPlus.Net.Microservice.Domain.ValueObjects;

namespace CodeDesignPlus.Net.Microservice.Domain
{
    public class OrderAggregate(Guid id) : AggregateRoot(id), IAuditTrail
    {
        public DateTime? CompletedOn { get; private set; }
        public DateTime? CancelledOn { get; private set; }
        public ClientEntity Client { get; private set; } = default!;
        public List<ProductEntity> Products { get; private set; } = [];
        public OrderStatus Status { get; private set; }
        public string? ReasonForCancellation { get; private set; }
        
        public Guid CreateBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? UpdateBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // TODO: Change Setter to private set from interface IAuditTrail - Repply - Add Interface IAuditTrail


        public static OrderAggregate Create(Guid id, Guid idClient, string nameClient, Guid tenant)
        {
            DomainGuard.GuidIsEmpty(id, Errors.IdOrderIsInvalid);
            DomainGuard.GuidIsEmpty(idClient, Errors.IdClientIsInvalid);
            DomainGuard.IsNullOrEmpty(nameClient, Errors.NameClientIsInvalid);
            DomainGuard.GuidIsEmpty(tenant, Errors.TenantIsInvalid);

            var client = new ClientEntity
            {
                Id = idClient,
                Name = nameClient
            };

            var @event = OrderCreatedDomainEvent.Create(id, client, tenant);

            var aggregate = new OrderAggregate(id)
            {
                Client = client,
                CreatedAt = @event.CreatedAt,
                Status = @event.OrderStatus,
                Tenant = tenant
            };

            aggregate.AddEvent(@event);

            return aggregate;
        }

        public void AddProduct(Guid id, string name, string description, long price, int quantity)
        {
            DomainGuard.GuidIsEmpty(id, Errors.IdProductIsInvalid);
            DomainGuard.IsNullOrEmpty(name, Errors.NameProductIsInvalid);
            DomainGuard.IsLessThan(price, 0, Errors.PriceProductIsInvalid);
            DomainGuard.IsLessThan(quantity, 0, Errors.QuantityProductIsInvalid);

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

        public void RemoveProduct(Guid productId)
        {
            DomainGuard.GuidIsEmpty(productId, Errors.IdProductIsInvalid);

            var product = Products.SingleOrDefault(x => x.Id == productId);

            DomainGuard.IsNull(product, Errors.ProductNotFound);

            Products.Remove(product);

            AddEvent(ProductRemovedFromOrderDomainEvent.Create(Id, productId));
        }

        public void UpdateProductQuantity(Guid productId, int newQuantity)
        {
            DomainGuard.GuidIsEmpty(productId, Errors.IdProductIsInvalid);
            DomainGuard.IsLessThan(newQuantity, 0, Errors.QuantityProductIsInvalid);

            var product = Products.SingleOrDefault(p => p.Id == productId);

            DomainGuard.IsNull(product, Errors.ProductNotFound);

            product.Quantity = newQuantity;

            AddEvent(ProductQuantityUpdatedDomainEvent.Create(Id, productId, newQuantity));
        }

        public void CompleteOrder()
        {
            DomainGuard.IsTrue(Status == OrderStatus.Completed, Errors.OrderAlreadyCompleted);

            var @event = OrderCompletedDomainEvent.Create(Id);

            this.CompletedOn = @event.CompletionDate;
            this.Status = OrderStatus.Completed;

            AddEvent(OrderCompletedDomainEvent.Create(Id));
        }

        public void CancelOrder(string reason)
        {
            DomainGuard.IsTrue(Status == OrderStatus.Cancelled, Errors.OrderAlreadyCancelled);

            this.ReasonForCancellation = reason;
            this.Status = OrderStatus.Cancelled;

            AddEvent(OrderCancelledDomainEvent.Create(Id, reason));
        }
    }
}
