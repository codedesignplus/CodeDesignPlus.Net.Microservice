using CodeDesignPlus.Net.Core.Abstractions;
using CodeDesignPlus.Net.Microservice.Domain.DomainEvents;
using CodeDesignPlus.Net.Microservice.Domain.Entities;
using CodeDesignPlus.Net.Microservice.Domain.ValueObjects;

namespace CodeDesignPlus.Net.Microservice.Domain
{
    public class OrderAggregate(Guid id) : AggregateRoot(id)
    {
        public DateTime? CompletionDate { get; private set; }
        public DateTime? CancellationDate { get; private set; }
        public ClientEntity Client { get; private set; } = default!;
        public List<ProductEntity> Products { get; private set; } = [];
        public OrderStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string? ReasonForCancellation { get; private set; }
        public Guid Tenant { get; private set; }

        // TODO: Change Setter to private set from interface IAuditTrail - Repply - Add Interface IAuditTrail


        public static OrderAggregate Create(Guid id, ClientEntity client, Guid tenant)
        {
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

        public void AddProduct(ProductEntity product, int quantity)
        {
            product.Quantity = quantity;

            Products.Add(product);

            AddEvent(ProductAddedToOrderDomainEvent.Create(Id, quantity, product));
        }

        public void RemoveProduct(Guid productId)
        {
            var product = Products.SingleOrDefault(x => x.Id == productId);

            if (product == null)
                throw new InvalidOperationException("Producto no encontrado en la orden.");

            Products.Remove(product);

            AddEvent(ProductRemovedFromOrderDomainEvent.Create(Id, productId));
        }

        public void UpdateProductQuantity(Guid productId, int newQuantity)
        {
            var product = Products.SingleOrDefault(p => p.Id == productId);

            if (product == null)
                throw new InvalidOperationException("Producto no encontrado en la orden.");

            product.Quantity = newQuantity;

            AddEvent(ProductQuantityUpdatedDomainEvent.Create(Id, productId, newQuantity));
        }

        public void CompleteOrder()
        {
            if (Status == OrderStatus.Completed)
                throw new InvalidOperationException("La orden ya está completada.");

            var @event = OrderCompletedDomainEvent.Create(Id);

            this.CompletionDate = @event.CompletionDate;
            this.Status = OrderStatus.Completed;

            AddEvent(OrderCompletedDomainEvent.Create(Id));
        }

        public void CancelOrder(string reason)
        {
            if (Status == OrderStatus.Cancelled)
                throw new InvalidOperationException("La orden ya está cancelada.");

            this.ReasonForCancellation = reason;
            this.Status = OrderStatus.Cancelled;

            AddEvent(OrderCancelledDomainEvent.Create(Id, reason));
        }
    }
}
