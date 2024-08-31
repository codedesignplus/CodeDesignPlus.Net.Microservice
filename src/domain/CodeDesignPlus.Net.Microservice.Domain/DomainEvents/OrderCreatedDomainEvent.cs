﻿
namespace CodeDesignPlus.Net.Microservice.Domain.DomainEvents;

[EventKey<OrderAggregate>(1, "OrderCreated")]
public class OrderCreatedDomainEvent(
   Guid aggregateId,
   OrderStatus orderStatus,
   ClientEntity client,
   long createdAt,
   Guid tenant,
   Guid? eventId = null,
   DateTime? occurredAt = null,
   Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public ClientEntity Client { get; } = client;
    public OrderStatus OrderStatus { get; } = orderStatus;
    public long CreatedAt { get; } = createdAt;
    public Guid Tenant { get; private set; } = tenant;

    public static OrderCreatedDomainEvent Create(Guid id, ClientEntity client, Guid tenant)
    {
        return new OrderCreatedDomainEvent(id, OrderStatus.Created, client, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), tenant);
    }
}

