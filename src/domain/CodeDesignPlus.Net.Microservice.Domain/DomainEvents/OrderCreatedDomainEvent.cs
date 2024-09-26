
namespace CodeDesignPlus.Net.Microservice.Domain.DomainEvents;

[EventKey<OrderAggregate>(1, "OrderCreated")]
public class OrderCreatedDomainEvent(
   Guid aggregateId,
   OrderStatus orderStatus,
   ClientEntity client,
   long createdAt,
   Guid tenant,
   Guid createBy,
   Guid? eventId = null,
   DateTime? occurredAt = null,
   Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public ClientEntity Client { get; } = client;
    public OrderStatus OrderStatus { get; } = orderStatus;
    public long CreatedAt { get; } = createdAt;
    public Guid Tenant { get; private set; } = tenant;
    public Guid CreateBy { get; private set; } = createBy;

    public static OrderCreatedDomainEvent Create(Guid id, ClientEntity client, Guid tenant, Guid creaateBy)
    {
        return new OrderCreatedDomainEvent(id, OrderStatus.Created, client, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), tenant, creaateBy);
    }
}

