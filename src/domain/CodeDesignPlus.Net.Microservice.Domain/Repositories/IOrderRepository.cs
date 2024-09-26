using CodeDesignPlus.Net.Mongo.Abstractions;

namespace CodeDesignPlus.Net.Microservice.Domain.Repositories;

public interface IOrderRepository : IRepositoryBase
{
    Task<OrderAggregate> FindAsync(Guid id, CancellationToken cancellationToken);
    Task<List<OrderAggregate>> GetAllOrdersAsync(CancellationToken cancellationToken);
    Task CreateOrderAsync(OrderAggregate order, CancellationToken cancellationToken);
    Task AddProductToOrderAsync(Guid idOrder, Guid idProduct, string name, string description, long price, int quantity, long? updaatedAt, Guid? updateBy, CancellationToken cancellationToken);
    Task RemoveProductFromOrderAsync(Guid idOrder, Guid idProduct, long? updaatedAt, Guid? updateBy, CancellationToken cancellationToken);
    Task UpdateQuantityProductAsync(Guid idOrder, Guid productId, int newQuantity, long? updaatedAt, Guid? updateBy, CancellationToken cancellationToken);
    Task CompleteOrderAsync(Guid idOrder, long? completedAt, OrderStatus orderStatus, long? updaatedAt, Guid? updateBy, CancellationToken cancellationToken);
    Task CancelOrderAsync(Guid idOrder, OrderStatus orderStatus, string? reason, long? cancelledAt, long? updaatedAt, Guid? updateBy, CancellationToken cancellationToken);
    Task UpdateOrderAsync(OrderAggregate order, CancellationToken cancellationToken);
}
