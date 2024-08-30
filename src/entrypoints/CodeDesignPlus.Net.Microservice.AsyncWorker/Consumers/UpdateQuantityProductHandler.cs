namespace CodeDesignPlus.Net.Microservice.AsyncWorker.Consumers;

[QueueName("productentity", "write_logger")]
public class UpdateQuantityProductHandler(ILogger<UpdateQuantityProductHandler> logger) : IEventHandler<ProductQuantityUpdatedDomainEvent>
{
    private readonly ILogger<UpdateQuantityProductHandler> logger = logger;

    public Task HandleAsync(ProductQuantityUpdatedDomainEvent data, CancellationToken token)
    {
        logger.LogInformation("ProductQuantityUpdatedDomainEvent Recived, {aggregateId}, {json}", data.AggregateId, JsonConvert.SerializeObject(data));

        return Task.CompletedTask;
    }
}
