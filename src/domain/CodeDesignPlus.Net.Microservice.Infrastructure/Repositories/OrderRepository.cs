namespace CodeDesignPlus.Net.Microservice.Infrastructure.Repositories;

public class OrderRepository(IServiceProvider serviceProvider, IOptions<MongoOptions> mongoOptions, ILogger<RepositoryBase> logger) 
    : RepositoryBase(serviceProvider, mongoOptions, logger), IOrderRepository
{
    public Task AddProductToOrderAsync(Guid id, Guid tenant, AddProductToOrderParams parameters, CancellationToken cancellationToken)
    {
        var product = new ProductEntity
        {
            Id = parameters.Id,
            Name = parameters.Name,
            Description = parameters.Description,
            Price = parameters.Price,
            Quantity = parameters.Quantity,
        };

        var filter = Builders<OrderAggregate>.Filter.And(
            Builders<OrderAggregate>.Filter.Eq(x => x.Id, id),
            Builders<OrderAggregate>.Filter.Eq(x => x.Tenant, tenant)
        );

        var update = Builders<OrderAggregate>.Update
            .Push(x => x.Products, product)
            .Set(x => x.UpdatedAt, parameters.UpdatedAt)
            .Set(x => x.UpdatedBy, parameters.UpdateBy);

        var collection = base.GetCollection<OrderAggregate>();

        return collection.UpdateOneAsync(
            filter,
            update,
            cancellationToken: cancellationToken
         );
    }

    public Task CancelOrderAsync(CancelOrderParams parameters, Guid tenant, CancellationToken cancellationToken)
    {
        var filter = Builders<OrderAggregate>.Filter.And(
            Builders<OrderAggregate>.Filter.Eq(x => x.Id, parameters.Id),
            Builders<OrderAggregate>.Filter.Eq(x => x.Tenant, tenant)
        );

        var update = Builders<OrderAggregate>.Update
            .Set(x => x.Status, parameters.OrderStatus)
            .Set(x => x.ReasonForCancellation, parameters.Reason)
            .Set(x => x.CancelledAt, parameters.CancelledAt)
            .Set(x => x.UpdatedAt, parameters.UpdatedAt)
            .Set(x => x.UpdatedBy, parameters.UpdateBy);

        return base.GetCollection<OrderAggregate>().UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }

    public Task CompleteOrderAsync(CompleteOrderParams parameters, Guid tenant, CancellationToken cancellationToken)
    {
        var filterId = Builders<OrderAggregate>.Filter.And(
            Builders<OrderAggregate>.Filter.Eq(x => x.Id, parameters.Id),
            Builders<OrderAggregate>.Filter.Eq(x => x.Tenant, tenant)
        );

        var update = Builders<OrderAggregate>.Update
            .Set(x => x.CompletedAt, parameters.CompletedAt)
            .Set(x => x.Status, parameters.OrderStatus)
            .Set(x => x.UpdatedAt, parameters.UpdatedAt)
            .Set(x => x.UpdatedBy, parameters.UpdateBy);

        return base.GetCollection<OrderAggregate>().UpdateOneAsync(filterId, update, cancellationToken: cancellationToken);
    }

    public Task RemoveProductFromOrderAsync(RemoveProductFromOrderParams parameters, Guid tenant, CancellationToken cancellationToken)
    {
        var filterId = Builders<OrderAggregate>.Filter.And(
            Builders<OrderAggregate>.Filter.Eq(x => x.Id, parameters.Id),
            Builders<OrderAggregate>.Filter.Eq(x => x.Tenant, tenant)
        );

        var update = Builders<OrderAggregate>.Update
            .PullFilter(x => x.Products, p => p.Id == parameters.IdProduct)
            .Set(x => x.UpdatedAt, parameters.UpdatedAt)
            .Set(x => x.UpdatedBy, parameters.UpdateBy);

        return base.GetCollection<OrderAggregate>().UpdateOneAsync(filterId, update, cancellationToken: cancellationToken);
    }

    public Task UpdateQuantityProductAsync(Guid id, Guid tenant, UpdateQuantityProductParams parameters, CancellationToken cancellationToken)
    {
        var filterId = Builders<OrderAggregate>.Filter.And(
            Builders<OrderAggregate>.Filter.Eq(x => x.Id, id),
            Builders<OrderAggregate>.Filter.Eq(x => x.Tenant, tenant),
            Builders<OrderAggregate>.Filter.ElemMatch(x => x.Products, x => x.Id == parameters.Id)
        );

        var update = Builders<OrderAggregate>.Update
            .Set("Products.$.Quantity", parameters.NewQuantity)
            .Set(x => x.UpdatedAt, parameters.UpdatedAt)
            .Set(x => x.UpdatedBy, parameters.UpdateBy);

        return base.GetCollection<OrderAggregate>().UpdateOneAsync(filterId, update, cancellationToken: cancellationToken);
    }
}