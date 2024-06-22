namespace CodeDesignPlus.Net.Microservice.AsyncWorker.Consumers;

public class ProductRemovedFromOrderHandler : IEventHandler<ProductRemovedFromOrderDomainEvent>
{
    private readonly ILogger<ProductRemovedFromOrderHandler> logger;

    public ProductRemovedFromOrderHandler(ILogger<ProductRemovedFromOrderHandler> logger)
    {
        this.logger = logger;
    }

    public Task HandleAsync(ProductRemovedFromOrderDomainEvent data, CancellationToken token)
    {
        logger.LogInformation("ProductRemovedFromOrderDomainEvent Recived, {aggregateId}, {json}", data.AggregateId, JsonConvert.SerializeObject(data));

        return Task.CompletedTask;
    }
}
