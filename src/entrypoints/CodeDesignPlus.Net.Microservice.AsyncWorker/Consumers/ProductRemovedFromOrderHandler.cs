namespace CodeDesignPlus.Net.Microservice.AsyncWorker.Consumers;

[QueueName("productentity", "product_removed")]
public class ProductRemovedFromOrderHandler(ILogger<ProductRemovedFromOrderHandler> logger) : IEventHandler<ProductRemovedFromOrderDomainEvent>
{
    private readonly ILogger<ProductRemovedFromOrderHandler> logger = logger;

    public Task HandleAsync(ProductRemovedFromOrderDomainEvent data, CancellationToken token)
    {
        logger.LogInformation("ProductRemovedFromOrderDomainEvent Recived, {AggregateId}, {Json}", data.AggregateId, JsonSerializer.Serialize(data));

        return Task.CompletedTask;
    }
}
