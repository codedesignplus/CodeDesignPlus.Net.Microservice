using CodeDesignPlus.Net.Microservice.Domain.DomainEvents;
using CodeDesignPlus.Net.PubSub.Abstractions;
using Newtonsoft.Json;

namespace CodeDesignPlus.Net.Microservice.Consumers
{
    public class CancelOrderHandler: IEventHandler<OrderCancelledDomainEvent>
    {
        private readonly ILogger<CancelOrderHandler> logger;

        public CancelOrderHandler(ILogger<CancelOrderHandler> logger)
        {
            this.logger = logger;
        }

        public Task HandleAsync(OrderCancelledDomainEvent data, CancellationToken token)
        {
            this.logger.LogInformation("OrderCancelledDomainEvent Recived, {aggregateId}, {json}", data.AggregateId, JsonConvert.SerializeObject(data));

            return Task.CompletedTask;
        }
    }
}
