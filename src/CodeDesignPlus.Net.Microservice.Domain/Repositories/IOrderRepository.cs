using CodeDesignPlus.Net.Microservice.Domain.Entities;
using CodeDesignPlus.Net.Mongo.Abstractions;

namespace CodeDesignPlus.Net.Microservice.Domain.Repositories
{
    // TODO: Add to IOperationBase resitriction IAggregateRoot or add IEntity to IAggregateRoot
    public interface IOrderRepository : IRepositoryBase
    {
        Task<OrderAggregate> FindAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<OrderAggregate>> GetAllOrdersAsync(CancellationToken cancellationToken = default);
        Task CreateOrderAsync(Guid id, ClientEntity client, Guid tenant, CancellationToken cancellationToken = default);
        Task AddProductToOrderAsync(Guid idOrder, ProductEntity product, int quantity, CancellationToken cancellationToken = default);
        Task RemoveProductFromOrderAsync(Guid idOrder, Guid idProduct, CancellationToken cancellationToken = default);
        Task UpdateQuantityProductAsync(Guid idOrder, Guid productId, int newQuantity, CancellationToken cancellationToken = default);
        Task CompleteOrderAsync(Guid idOrder, DateTime completionDate, CancellationToken cancellationToken = default);
        Task CancelOrderAsync(Guid idOrder, string reason, CancellationToken cancellationToken = default);
        Task UpdateOrderAsync(OrderAggregate order, CancellationToken cancellationToken = default);
    }
}
