using CodeDesignPlus.Net.Microservice.Domain;
using CodeDesignPlus.Net.Microservice.Domain.Entities;
using CodeDesignPlus.Net.Microservice.Domain.Repositories;
using CodeDesignPlus.Net.Microservice.Domain.ValueObjects;
using CodeDesignPlus.Net.Mongo.Abstractions.Options;
using CodeDesignPlus.Net.Mongo.Repository;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CodeDesignPlus.Net.Microservice.Infrastructure.Repositories
{
    public class OrderRepository : RepositoryBase, IOrderRepository
    {
        public OrderRepository(IServiceProvider serviceProvider, IOptions<MongoOptions> mongoOptions, ILogger<RepositoryBase> logger)
            : base(serviceProvider, mongoOptions, logger)
        {
        }

        public Task AddProductToOrderAsync(Guid idOrder, ProductEntity product, int quantity, CancellationToken cancellationToken = default)
        {
            product.Quantity = quantity;

            var filterId = Builders<OrderAggregate>.Filter.Eq(x => x.Id, idOrder);
            var update = Builders<OrderAggregate>.Update.Push(x => x.Products, product);

            return base.GetCollection<OrderAggregate>()
                .UpdateOneAsync(
                    filterId,
                    update,
                    cancellationToken: cancellationToken
                 );
        }

        public Task CancelOrderAsync(Guid idOrder, string reason, CancellationToken cancellationToken = default)
        {
            var filterId = Builders<OrderAggregate>.Filter.Eq(x => x.Id, idOrder);

            var update = Builders<OrderAggregate>.Update
                .Set(x => x.Status, OrderStatus.Cancelled)
                .Set(x => x.ReasonForCancellation, reason);

            return base.GetCollection<OrderAggregate>().UpdateOneAsync(filterId, update, cancellationToken: cancellationToken);
        }

        public Task CompleteOrderAsync(Guid idOrder, DateTime completionDate, CancellationToken cancellationToken = default)
        {
            var filterId = Builders<OrderAggregate>.Filter.Eq(x => x.Id, idOrder);

            var update = Builders<OrderAggregate>.Update.Set(x => x.CompletionDate, completionDate);

            return base.GetCollection<OrderAggregate>().UpdateOneAsync(filterId, update, cancellationToken: cancellationToken);
        }

        public Task CreateOrderAsync(Guid id, ClientEntity client, Guid tenant, CancellationToken cancellationToken = default)
        {
            var order = OrderAggregate.Create(id, client, tenant);

            return this.CreateAsync(order, cancellationToken);
        }

        public Task<OrderAggregate> FindAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var filterId = Builders<OrderAggregate>.Filter.Eq(x => x.Id, id);

            return base.GetCollection<OrderAggregate>().FindAsync(filterId, cancellationToken: cancellationToken).Result.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<OrderAggregate>> GetAllOrdersAsync(CancellationToken cancellationToken = default)
        {
            var result = await base.GetCollection<OrderAggregate>().FindAsync(x => true, cancellationToken: cancellationToken);

            return await result.ToListAsync(cancellationToken);
        }

        public Task RemoveProductFromOrderAsync(Guid idOrder, Guid idProduct, CancellationToken cancellationToken = default)
        {
            var filterId = Builders<OrderAggregate>.Filter.Eq(x => x.Id, idOrder);

            var update = Builders<OrderAggregate>.Update.PullFilter(x => x.Products, p => p.Id == idProduct);

            return base.GetCollection<OrderAggregate>().UpdateOneAsync(filterId, update, cancellationToken: cancellationToken);
        }

        public Task UpdateOrderAsync(OrderAggregate order, CancellationToken cancellationToken = default)
        {
            return this.UpdateAsync(order, cancellationToken);
        }

        public Task UpdateQuantityProductAsync(Guid idOrder, Guid productId, int newQuantity, CancellationToken cancellationToken = default)
        {
            var filterId = Builders<OrderAggregate>.Filter.And(
                Builders<OrderAggregate>.Filter.Eq(x => x.Id, idOrder),
                Builders<OrderAggregate>.Filter.ElemMatch(x => x.Products, x => x.Id == productId)
            );

            var update = Builders<OrderAggregate>.Update.Set(x => x.Products[-1].Quantity, newQuantity);

            return base.GetCollection<OrderAggregate>().UpdateOneAsync(filterId, update, cancellationToken: cancellationToken);

        }
    }
}
