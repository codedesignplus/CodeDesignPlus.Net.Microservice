
namespace CodeDesignPlus.Net.Microservice.Domain.DomainEvents;

[Key("dtop.ms-archetype.v1.domain_event.order.created")]
public class OrderCreatedDomainEvent(
   Guid aggregateId,
   OrderStatus orderStatus,
   ClientEntity client,
   long createAt,
   Guid tenant,
   Guid? eventId = null,
   DateTime? occurredAt = null,
   Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public ClientEntity Client { get; } = client;
    public OrderStatus OrderStatus { get; } = orderStatus;
    public long CreatedAt { get; } = createAt;
    public Guid Tenant { get; private set; } = tenant;

    public static OrderCreatedDomainEvent Create(Guid id, ClientEntity client, Guid tenant)
    {
        return new OrderCreatedDomainEvent(id, OrderStatus.Created, client, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), tenant);
    }
}

