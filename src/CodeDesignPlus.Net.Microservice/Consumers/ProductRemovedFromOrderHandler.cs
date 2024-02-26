using CodeDesignPlus.Net.Microservice.Domain.DomainEvents;
using CodeDesignPlus.Net.PubSub.Abstractions;
using Newtonsoft.Json;

namespace CodeDesignPlus.Net.Microservice.Consumers
{
    public class ProductRemovedFromOrderHandler: IEventHandler<ProductRemovedFromOrderDomainEvent>
    {
        private readonly ILogger<ProductRemovedFromOrderHandler> logger;

        public ProductRemovedFromOrderHandler(ILogger<ProductRemovedFromOrderHandler> logger)
        {
            this.logger = logger;
        }

        public Task HandleAsync(ProductRemovedFromOrderDomainEvent data, CancellationToken token)
        {
            this.logger.LogInformation("ProductRemovedFromOrderDomainEvent Recived, {aggregateId}, {json}", data.AggregateId, JsonConvert.SerializeObject(data));

            return Task.CompletedTask;
        }
    }
}
