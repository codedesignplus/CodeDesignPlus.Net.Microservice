namespace CodeDesignPlus.Net.Microservice.AsyncWorker.Consumers;

[QueueName("productentity", "update_quantity")]
public class UpdateQuantityProductHandler(ILogger<UpdateQuantityProductHandler> logger) : IEventHandler<ProductQuantityUpdatedDomainEvent>
{
    private readonly ILogger<UpdateQuantityProductHandler> logger = logger;

    public Task HandleAsync(ProductQuantityUpdatedDomainEvent data, CancellationToken token)
    {
        logger.LogInformation("ProductQuantityUpdatedDomainEvent Recived, {AggregateId}, {Json}", data.AggregateId, JsonConvert.SerializeObject(data));

        return Task.CompletedTask;
    }
}
