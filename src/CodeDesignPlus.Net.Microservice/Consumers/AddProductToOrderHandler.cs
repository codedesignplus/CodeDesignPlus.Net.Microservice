using CodeDesignPlus.Net.Microservice.Domain.DomainEvents;
using CodeDesignPlus.Net.PubSub.Abstractions;
using Newtonsoft.Json;

namespace CodeDesignPlus.Net.Microservice.Consumers
{
    public class AddProductToOrderHandler : IEventHandler<ProductAddedToOrderDomainEvent>
    {
        private readonly ILogger<AddProductToOrderHandler> logger;

        public AddProductToOrderHandler(ILogger<AddProductToOrderHandler> logger)
        {
            this.logger = logger;
        }

        public Task HandleAsync(ProductAddedToOrderDomainEvent data, CancellationToken token)
        {
            this.logger.LogInformation("ProductAddedToOrderDomainEvent Recived, {aggregateId}, {json}", data.AggregateId, JsonConvert.SerializeObject(data));

            return Task.CompletedTask;
        }
    }
}
