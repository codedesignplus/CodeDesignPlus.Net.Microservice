using CodeDesignPlus.Net.Microservice.Domain.DomainEvents;
using CodeDesignPlus.Net.PubSub.Abstractions;
using Newtonsoft.Json;

namespace CodeDesignPlus.Net.Microservice.Consumers
{
    public class UpdateQuantityProductHandler: IEventHandler<ProductQuantityUpdatedDomainEvent>
    {
        private readonly ILogger<UpdateQuantityProductHandler> logger;

        public UpdateQuantityProductHandler(ILogger<UpdateQuantityProductHandler> logger)
        {
            this.logger = logger;
        }

        public Task HandleAsync(ProductQuantityUpdatedDomainEvent data, CancellationToken token)
        {
            this.logger.LogInformation("ProductQuantityUpdatedDomainEvent Recived, {aggregateId}, {json}", data.AggregateId, JsonConvert.SerializeObject(data));

            return Task.CompletedTask;
        }
    }
}
