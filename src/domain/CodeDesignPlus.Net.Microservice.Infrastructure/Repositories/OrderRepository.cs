namespace CodeDesignPlus.Net.Microservice.Infrastructure.Repositories;

public class OrderRepository(IServiceProvider serviceProvider, IOptions<MongoOptions> mongoOptions, ILogger<RepositoryBase> logger) : RepositoryBase(serviceProvider, mongoOptions, logger), IOrderRepository
{
    public Task AddProductToOrderAsync(Guid idOrder, Guid idProduct, string name, string description, long price, int quantity, long? updaatedAt, Guid? updateBy, CancellationToken cancellationToken)
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
        var update = Builders<OrderAggregate>.Update
            .Push(x => x.Products, product)
            .Set(x => x.UpdatedAt, updaatedAt)
            .Set(x => x.UpdatedBy, updateBy);

        var collection = base.GetCollection<OrderAggregate>();

        return collection.UpdateOneAsync(
            filterId,
            update,
            cancellationToken: cancellationToken
         );
    }

    public Task CancelOrderAsync(Guid idOrder, OrderStatus orderStatus, string? reason, long? cancelledAt, long? updaatedAt, Guid? updateBy, CancellationToken cancellationToken)
    {
        var filterId = Builders<OrderAggregate>.Filter.Eq(x => x.Id, idOrder);

        var update = Builders<OrderAggregate>.Update
            .Set(x => x.Status, orderStatus)
            .Set(x => x.ReasonForCancellation, reason)
            .Set(x => x.CancelledAt, cancelledAt)
            .Set(x => x.UpdatedAt, updaatedAt)
            .Set(x => x.UpdatedBy, updateBy);

        return base.GetCollection<OrderAggregate>().UpdateOneAsync(filterId, update, cancellationToken: cancellationToken);
    }

    public Task CompleteOrderAsync(Guid idOrder, long? completedAt, OrderStatus orderStatus, long? updaatedAt, Guid? updateBy, CancellationToken cancellationToken)
    {
        var filterId = Builders<OrderAggregate>.Filter.Eq(x => x.Id, idOrder);

        var update = Builders<OrderAggregate>.Update
            .Set(x => x.CompletedAt, completedAt)
            .Set(x => x.Status, orderStatus)
            .Set(x => x.UpdatedAt, updaatedAt)
            .Set(x => x.UpdatedBy, updateBy);

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

    public Task RemoveProductFromOrderAsync(Guid idOrder, Guid idProduct, long? updaatedAt, Guid? updateBy, CancellationToken cancellationToken)
    {
        var filterId = Builders<OrderAggregate>.Filter.Eq(x => x.Id, idOrder);

        var update = Builders<OrderAggregate>.Update
            .PullFilter(x => x.Products, p => p.Id == idProduct)
            .Set(x => x.UpdatedAt, updaatedAt)
            .Set(x => x.UpdatedBy, updateBy);

        return base.GetCollection<OrderAggregate>().UpdateOneAsync(filterId, update, cancellationToken: cancellationToken);
    }

    public Task UpdateOrderAsync(OrderAggregate order, CancellationToken cancellationToken)
    {
        return this.UpdateAsync(order, cancellationToken);
    }

    public Task UpdateQuantityProductAsync(Guid idOrder, Guid productId, int newQuantity, long? updaatedAt, Guid? updateBy, CancellationToken cancellationToken)
    {
        var filterId = Builders<OrderAggregate>.Filter.And(
            Builders<OrderAggregate>.Filter.Eq(x => x.Id, idOrder),
            Builders<OrderAggregate>.Filter.ElemMatch(x => x.Products, x => x.Id == productId)
        );

        var update = Builders<OrderAggregate>.Update
            .Set("Products.$.Quantity", newQuantity)
            .Set(x => x.UpdatedAt, updaatedAt)
            .Set(x => x.UpdatedBy, updateBy);

        return base.GetCollection<OrderAggregate>().UpdateOneAsync(filterId, update, cancellationToken: cancellationToken);

    }
}
