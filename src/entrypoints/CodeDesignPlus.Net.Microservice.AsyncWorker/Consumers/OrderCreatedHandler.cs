namespace CodeDesignPlus.Net.Microservice.AsyncWorker.Consumers;

[QueueName("orderaggregate", "write_logger")]
public class OrderCreatedHandler(ILogger<OrderCreatedHandler> logger) : IEventHandler<OrderCreatedDomainEvent>
{
    private readonly ILogger<OrderCreatedHandler> logger = logger;

    public Task HandleAsync(OrderCreatedDomainEvent data, CancellationToken token)
    {
        logger.LogInformation("OrderCreatedDomainEvent Recived, {aggregateId}, {json}", data.AggregateId, JsonConvert.SerializeObject(data));

        return Task.CompletedTask;
    }
}

