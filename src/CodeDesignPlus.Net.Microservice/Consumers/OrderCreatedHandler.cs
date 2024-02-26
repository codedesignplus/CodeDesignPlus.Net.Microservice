using CodeDesignPlus.Net.Microservice.Domain.DomainEvents;
using CodeDesignPlus.Net.PubSub.Abstractions;
using Newtonsoft.Json;

namespace CodeDesignPlus.Net.Microservice.Consumers
{
    public class OrderCreatedHandler : IEventHandler<OrderCreatedDomainEvent>
    {
        private readonly ILogger<OrderCreatedHandler> logger;

        public OrderCreatedHandler(ILogger<OrderCreatedHandler> logger)
        {
            this.logger = logger;
        }

        public Task HandleAsync(OrderCreatedDomainEvent data, CancellationToken token)
        {
            this.logger.LogInformation("OrderCreatedDomainEvent Recived, {aggregateId}, {json}", data.AggregateId, JsonConvert.SerializeObject(data));

            return Task.CompletedTask;
        }
    }
}
