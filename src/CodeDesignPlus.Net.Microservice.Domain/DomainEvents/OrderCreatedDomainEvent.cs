using CodeDesignPlus.Net.Core.Abstractions;
using CodeDesignPlus.Net.Core.Abstractions.Attributes;
using CodeDesignPlus.Net.Microservice.Domain.Entities;
using CodeDesignPlus.Net.Microservice.Domain.ValueObjects;

namespace CodeDesignPlus.Net.Microservice.Domain.DomainEvents
{
    [Key("dtop.ms-archetype.v1.domain_event.order.created")]
    public class OrderCreatedDomainEvent(
       Guid aggregateId,
       OrderStatus orderStatus,
       ClientEntity client,
       DateTime dateCreated,
       Guid tenant,
       Guid? eventId = null,
       DateTime? occurredAt = null,
       Dictionary<string, object>? metadata = null
   ) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
    {
        public ClientEntity Client { get; } = client;
        public OrderStatus OrderStatus { get; } = orderStatus;
        public DateTime CreatedAt { get; } = dateCreated;
        public Guid Tenant { get; private set; } = tenant;

        public static OrderCreatedDomainEvent Create(Guid id, ClientEntity client, Guid tenant)
        {
            return new OrderCreatedDomainEvent(id, OrderStatus.Created, client, DateTime.UtcNow, tenant);
        }
    }
}
