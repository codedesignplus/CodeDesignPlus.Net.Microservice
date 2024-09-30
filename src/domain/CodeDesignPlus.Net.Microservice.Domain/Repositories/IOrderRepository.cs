namespace CodeDesignPlus.Net.Microservice.Domain.Repositories;

public interface IOrderRepository : IRepositoryBase
{
    Task<OrderAggregate> FindAsync(Guid id, CancellationToken cancellationToken);
    Task<List<OrderAggregate>> GetAllOrdersAsync(CancellationToken cancellationToken);
    Task CreateOrderAsync(OrderAggregate order, CancellationToken cancellationToken);
    Task AddProductToOrderAsync(AddProductToOrderParams parameters, CancellationToken cancellationToken);
    Task RemoveProductFromOrderAsync(RemoveProductFromOrderParams parameters, CancellationToken cancellationToken);
    Task UpdateQuantityProductAsync(UpdateQuantityProductParams parameters, CancellationToken cancellationToken);
    Task CompleteOrderAsync(CompleteOrderParams parameters, CancellationToken cancellationToken);
    Task CancelOrderAsync(CancelOrderParams parameters, CancellationToken cancellationToken);
    Task UpdateOrderAsync(OrderAggregate order, CancellationToken cancellationToken);
}