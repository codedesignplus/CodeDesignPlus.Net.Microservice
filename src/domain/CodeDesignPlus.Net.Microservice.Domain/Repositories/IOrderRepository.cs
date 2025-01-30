namespace CodeDesignPlus.Net.Microservice.Domain.Repositories;

public interface IOrderRepository : IRepositoryBase
{
    Task AddProductToOrderAsync(Guid id, Guid tenant, AddProductToOrderParams parameters, CancellationToken cancellationToken);
    Task CancelOrderAsync(CancelOrderParams parameters, Guid tenant, CancellationToken cancellationToken);
    Task CompleteOrderAsync(CompleteOrderParams parameters, Guid tenant, CancellationToken cancellationToken);
    Task RemoveProductFromOrderAsync(RemoveProductFromOrderParams parameters, Guid tenant, CancellationToken cancellationToken);
    Task UpdateQuantityProductAsync(Guid id, Guid tenant, UpdateQuantityProductParams parameters, CancellationToken cancellationToken);
}