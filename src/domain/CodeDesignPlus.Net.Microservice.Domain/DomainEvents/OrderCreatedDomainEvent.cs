﻿using CodeDesignPlus.Net.Microservice.Domain.ValueObjects;

namespace CodeDesignPlus.Net.Microservice.Domain.DomainEvents;

[EventKey<OrderAggregate>(1, "OrderCreated")]
public class OrderCreatedDomainEvent(
   Guid aggregateId,
   OrderStatus orderStatus,
   ClientValueObject client,
   AddressValueObject shippingAddress,
   Instant createdAt,
   Guid tenant,
   Guid createBy,
   Guid? eventId = null,
   Instant? occurredAt = null,
   Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public ClientValueObject Client { get; } = client;
    public AddressValueObject ShippingAddress { get; } = shippingAddress;
    public OrderStatus OrderStatus { get; } = orderStatus;
    public Instant CreatedAt { get; } = createdAt;
    public Guid Tenant { get; private set; } = tenant;
    public Guid CreateBy { get; private set; } = createBy;

    public static OrderCreatedDomainEvent Create(
        Guid id,
        ClientValueObject client,
        AddressValueObject shippingAddress,
        Guid tenant,
        Guid creaateBy)
    {
        return new OrderCreatedDomainEvent(
            id,
            OrderStatus.Created,
            client,
            shippingAddress,
            SystemClock.Instance.GetCurrentInstant(),
            tenant,
            creaateBy
        );
    }
}

