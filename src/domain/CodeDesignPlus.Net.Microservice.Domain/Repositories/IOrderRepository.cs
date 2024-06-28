using CodeDesignPlus.Net.Mongo.Abstractions;

namespace CodeDesignPlus.Net.Microservice.Domain.Repositories;

public interface IOrderRepository : IRepositoryBase
{
    Task<OrderAggregate> FindAsync(Guid id, CancellationToken cancellationToken);
    Task<List<OrderAggregate>> GetAllOrdersAsync(CancellationToken cancellationToken);
    Task CreateOrderAsync(OrderAggregate order, CancellationToken cancellationToken);
    Task AddProductToOrderAsync(Guid idOrder, Guid idProduct, string name, string description, long price, int quantity, CancellationToken cancellationToken);
    Task RemoveProductFromOrderAsync(Guid idOrder, Guid idProduct, CancellationToken cancellationToken);
    Task UpdateQuantityProductAsync(Guid idOrder, Guid productId, int newQuantity, CancellationToken cancellationToken);
    Task CompleteOrderAsync(Guid idOrder, long? completedAt, CancellationToken cancellationToken);
    Task CancelOrderAsync(Guid idOrder, string reason, CancellationToken cancellationToken);
    Task UpdateOrderAsync(OrderAggregate order, CancellationToken cancellationToken);
}
