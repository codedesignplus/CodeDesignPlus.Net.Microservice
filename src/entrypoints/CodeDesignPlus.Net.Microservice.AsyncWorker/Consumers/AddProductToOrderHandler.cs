namespace CodeDesignPlus.Net.Microservice.AsyncWorker.Consumers;

[QueueName("productentity", "write_logger")]
public class AddProductToOrderHandler(ILogger<AddProductToOrderHandler> logger) : IEventHandler<ProductAddedToOrderDomainEvent>
{
    private readonly ILogger<AddProductToOrderHandler> logger = logger;

    public Task HandleAsync(ProductAddedToOrderDomainEvent data, CancellationToken token)
    {
        logger.LogInformation("ProductAddedToOrderDomainEvent Recived, {aggregateId}, {json}", data.AggregateId, JsonConvert.SerializeObject(data));

        return Task.CompletedTask;
    }
}
