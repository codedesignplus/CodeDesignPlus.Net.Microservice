namespace CodeDesignPlus.Net.Microservice.AsyncWorker.Consumers;

public class UpdateQuantityProductHandler : IEventHandler<ProductQuantityUpdatedDomainEvent>
{
    private readonly ILogger<UpdateQuantityProductHandler> logger;

    public UpdateQuantityProductHandler(ILogger<UpdateQuantityProductHandler> logger)
    {
        this.logger = logger;
    }

    public Task HandleAsync(ProductQuantityUpdatedDomainEvent data, CancellationToken token)
    {
        logger.LogInformation("ProductQuantityUpdatedDomainEvent Recived, {aggregateId}, {json}", data.AggregateId, JsonConvert.SerializeObject(data));

        return Task.CompletedTask;
    }
}
