namespace CodeDesignPlus.Net.Microservice.AsyncWorker.Consumers;

[QueueName("orderaggregate", "write_logger")]
public class ProductRemovedFromOrderHandler(ILogger<ProductRemovedFromOrderHandler> logger) : IEventHandler<ProductRemovedFromOrderDomainEvent>
{
    private readonly ILogger<ProductRemovedFromOrderHandler> logger = logger;

    public Task HandleAsync(ProductRemovedFromOrderDomainEvent data, CancellationToken token)
    {
        logger.LogInformation("ProductRemovedFromOrderDomainEvent Recived, {aggregateId}, {json}", data.AggregateId, JsonConvert.SerializeObject(data));

        return Task.CompletedTask;
    }
}
