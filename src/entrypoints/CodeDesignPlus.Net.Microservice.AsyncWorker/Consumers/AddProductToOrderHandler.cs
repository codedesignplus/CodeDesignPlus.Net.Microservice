namespace CodeDesignPlus.Net.Microservice.AsyncWorker.Consumers;

[QueueName("productentity", "app_product")]
public class AddProductToOrderHandler(ILogger<AddProductToOrderHandler> logger) : IEventHandler<ProductAddedToOrderDomainEvent>
{
    private readonly ILogger<AddProductToOrderHandler> logger = logger;

    public Task HandleAsync(ProductAddedToOrderDomainEvent data, CancellationToken token)
    {
        logger.LogInformation("ProductAddedToOrderDomainEvent Recived, {AggregateId}, {Json}", data.AggregateId, JsonConvert.SerializeObject(data));

        return Task.CompletedTask;
    }
}
