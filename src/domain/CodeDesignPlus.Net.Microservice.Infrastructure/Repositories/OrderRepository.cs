namespace CodeDesignPlus.Net.Microservice.Infrastructure.Repositories;

public class OrderRepository(IServiceProvider serviceProvider, IOptions<MongoOptions> mongoOptions, ILogger<RepositoryBase> logger) : RepositoryBase(serviceProvider, mongoOptions, logger), IOrderRepository
{
    public Task AddProductToOrderAsync(Guid idOrder, Guid idProduct, string name, string description, long price, int quantity, CancellationToken cancellationToken)
    {
        var product = new ProductEntity
        {
            Id = idProduct,
            Name = name,
            Description = description,
            Price = price,
            Quantity = quantity
        };

        var filterId = Builders<OrderAggregate>.Filter.Eq(x => x.Id, idOrder);
        var update = Builders<OrderAggregate>.Update.Push(x => x.Products, product);

        var collection = base.GetCollection<OrderAggregate>();

        return collection.UpdateOneAsync(
            filterId,
            update,
            cancellationToken: cancellationToken
         );
    }

    public Task CancelOrderAsync(Guid idOrder, string reason, CancellationToken cancellationToken)
    {
        var filterId = Builders<OrderAggregate>.Filter.Eq(x => x.Id, idOrder);

        var update = Builders<OrderAggregate>.Update
            .Set(x => x.Status, OrderStatus.Cancelled)
            .Set(x => x.ReasonForCancellation, reason);

        return base.GetCollection<OrderAggregate>().UpdateOneAsync(filterId, update, cancellationToken: cancellationToken);
    }

    public Task CompleteOrderAsync(Guid idOrder, long? completedAt, CancellationToken cancellationToken)
    {
        var filterId = Builders<OrderAggregate>.Filter.Eq(x => x.Id, idOrder);

        var update = Builders<OrderAggregate>.Update.Set(x => x.CompletedAt, completedAt);

        return base.GetCollection<OrderAggregate>().UpdateOneAsync(filterId, update, cancellationToken: cancellationToken);
    }

    public Task CreateOrderAsync(OrderAggregate order, CancellationToken cancellationToken)
    {
        return this.CreateAsync(order, cancellationToken);
    }

    public Task<OrderAggregate> FindAsync(Guid id, CancellationToken cancellationToken)
    {
        var filterId = Builders<OrderAggregate>.Filter.Eq(x => x.Id, id);

        return base.GetCollection<OrderAggregate>().FindAsync(filterId, cancellationToken: cancellationToken).Result.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<OrderAggregate>> GetAllOrdersAsync(CancellationToken cancellationToken)
    {
        var result = await base.GetCollection<OrderAggregate>().FindAsync(x => true, cancellationToken: cancellationToken);

        return await result.ToListAsync(cancellationToken);
    }

    public Task RemoveProductFromOrderAsync(Guid idOrder, Guid idProduct, CancellationToken cancellationToken)
    {
        var filterId = Builders<OrderAggregate>.Filter.Eq(x => x.Id, idOrder);

        var update = Builders<OrderAggregate>.Update.PullFilter(x => x.Products, p => p.Id == idProduct);

        return base.GetCollection<OrderAggregate>().UpdateOneAsync(filterId, update, cancellationToken: cancellationToken);
    }

    public Task UpdateOrderAsync(OrderAggregate order, CancellationToken cancellationToken)
    {
        return this.UpdateAsync(order, cancellationToken);
    }

    public Task UpdateQuantityProductAsync(Guid idOrder, Guid productId, int newQuantity, CancellationToken cancellationToken)
    {
        var filterId = Builders<OrderAggregate>.Filter.And(
            Builders<OrderAggregate>.Filter.Eq(x => x.Id, idOrder),
            Builders<OrderAggregate>.Filter.ElemMatch(x => x.Products, x => x.Id == productId)
        );

        var update = Builders<OrderAggregate>.Update.Set(x => x.Products[-1].Quantity, newQuantity);

        return base.GetCollection<OrderAggregate>().UpdateOneAsync(filterId, update, cancellationToken: cancellationToken);

    }
}
