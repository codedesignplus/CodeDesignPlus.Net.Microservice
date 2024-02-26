using CodeDesignPlus.Net.Microservice.Domain.DomainEvents;
using CodeDesignPlus.Net.PubSub.Abstractions;
using Newtonsoft.Json;

namespace CodeDesignPlus.Net.Microservice.Consumers
{
    public class CompleteOrderHandler: IEventHandler<OrderCompletedDomainEvent>
    {
        private readonly ILogger<CompleteOrderHandler> logger;

        public CompleteOrderHandler(ILogger<CompleteOrderHandler> logger)
        {
            this.logger = logger;
        }

        public Task HandleAsync(OrderCompletedDomainEvent data, CancellationToken token)
        {
            this.logger.LogInformation("OrderCompletedDomainEvent Recived, {aggregateId}, {json}", data.AggregateId, JsonConvert.SerializeObject(data));

            return Task.CompletedTask;
        }
    }
}
