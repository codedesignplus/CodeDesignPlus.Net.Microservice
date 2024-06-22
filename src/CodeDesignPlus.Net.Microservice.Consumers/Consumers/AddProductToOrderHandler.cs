namespace CodeDesignPlus.Net.Microservice.AsyncWorker.Consumers;

public class AddProductToOrderHandler : IEventHandler<ProductAddedToOrderDomainEvent>
{
    private readonly ILogger<AddProductToOrderHandler> logger;

    public AddProductToOrderHandler(ILogger<AddProductToOrderHandler> logger)
    {
        this.logger = logger;
    }

    public Task HandleAsync(ProductAddedToOrderDomainEvent data, CancellationToken token)
    {
        logger.LogInformation("ProductAddedToOrderDomainEvent Recived, {aggregateId}, {json}", data.AggregateId, JsonConvert.SerializeObject(data));

        return Task.CompletedTask;
    }
}
