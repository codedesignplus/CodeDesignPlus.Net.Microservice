namespace CodeDesignPlus.Net.Microservice.AsyncWorker.Consumers;

[QueueName("orderaggregate", "create_order")]
public class OrderCreatedHandler(ILogger<OrderCreatedHandler> logger) : IEventHandler<OrderCreatedDomainEvent>
{
    private readonly ILogger<OrderCreatedHandler> logger = logger;

    public Task HandleAsync(OrderCreatedDomainEvent data, CancellationToken token)
    {
        logger.LogInformation("OrderCreatedDomainEvent Recived, {AggregateId}, {Json}", data.AggregateId, JsonConvert.SerializeObject(data));

        return Task.CompletedTask;
    }
}

