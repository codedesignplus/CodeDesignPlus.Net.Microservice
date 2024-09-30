namespace CodeDesignPlus.Net.Microservice.Infrastructure.Repositories;

public class OrderRepository(IServiceProvider serviceProvider, IOptions<MongoOptions> mongoOptions, ILogger<RepositoryBase> logger) 
    : RepositoryBase(serviceProvider, mongoOptions, logger), IOrderRepository
{
    public Task AddProductToOrderAsync(AddProductToOrderParams parameters, CancellationToken cancellationToken)
    {
        var product = new ProductEntity
        {
            Id = parameters.IdProduct,
            Name = parameters.Name,
            Description = parameters.Description,
            Price = parameters.Price,
            Quantity = parameters.Quantity
        };

        var filterId = Builders<OrderAggregate>.Filter.Eq(x => x.Id, parameters.Id);
        var update = Builders<OrderAggregate>.Update
            .Push(x => x.Products, product)
            .Set(x => x.UpdatedAt, parameters.UpdatedAt)
            .Set(x => x.UpdatedBy, parameters.UpdateBy);

        var collection = base.GetCollection<OrderAggregate>();

        return collection.UpdateOneAsync(
            filterId,
            update,
            cancellationToken: cancellationToken
         );
    }

    public Task CancelOrderAsync(CancelOrderParams parameters, CancellationToken cancellationToken)
    {
        var filterId = Builders<OrderAggregate>.Filter.Eq(x => x.Id, parameters.Id);

        var update = Builders<OrderAggregate>.Update
            .Set(x => x.Status, parameters.OrderStatus)
            .Set(x => x.ReasonForCancellation, parameters.Reason)
            .Set(x => x.CancelledAt, parameters.CancelledAt)
            .Set(x => x.UpdatedAt, parameters.UpdatedAt)
            .Set(x => x.UpdatedBy, parameters.UpdateBy);

        return base.GetCollection<OrderAggregate>().UpdateOneAsync(filterId, update, cancellationToken: cancellationToken);
    }

    public Task CompleteOrderAsync(CompleteOrderParams parameters, CancellationToken cancellationToken)
    {
        var filterId = Builders<OrderAggregate>.Filter.Eq(x => x.Id, parameters.Id);

        var update = Builders<OrderAggregate>.Update
            .Set(x => x.CompletedAt, parameters.CompletedAt)
            .Set(x => x.Status, parameters.OrderStatus)
            .Set(x => x.UpdatedAt, parameters.UpdatedAt)
            .Set(x => x.UpdatedBy, parameters.UpdateBy);

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

    public Task RemoveProductFromOrderAsync(RemoveProductFromOrderParams parameters, CancellationToken cancellationToken)
    {
        var filterId = Builders<OrderAggregate>.Filter.Eq(x => x.Id, parameters.Id);

        var update = Builders<OrderAggregate>.Update
            .PullFilter(x => x.Products, p => p.Id == parameters.IdProduct)
            .Set(x => x.UpdatedAt, parameters.UpdatedAt)
            .Set(x => x.UpdatedBy, parameters.UpdateBy);

        return base.GetCollection<OrderAggregate>().UpdateOneAsync(filterId, update, cancellationToken: cancellationToken);
    }

    public Task UpdateOrderAsync(OrderAggregate order, CancellationToken cancellationToken)
    {
        return this.UpdateAsync(order, cancellationToken);
    }

    public Task UpdateQuantityProductAsync(UpdateQuantityProductParams parameters, CancellationToken cancellationToken)
    {
        var filterId = Builders<OrderAggregate>.Filter.And(
            Builders<OrderAggregate>.Filter.Eq(x => x.Id, parameters.Id),
            Builders<OrderAggregate>.Filter.ElemMatch(x => x.Products, x => x.Id == parameters.ProductId)
        );

        var update = Builders<OrderAggregate>.Update
            .Set("Products.$.Quantity", parameters.NewQuantity)
            .Set(x => x.UpdatedAt, parameters.UpdatedAt)
            .Set(x => x.UpdatedBy, parameters.UpdateBy);

        return base.GetCollection<OrderAggregate>().UpdateOneAsync(filterId, update, cancellationToken: cancellationToken);
    }
}