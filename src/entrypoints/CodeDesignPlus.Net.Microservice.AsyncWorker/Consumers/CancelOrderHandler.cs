namespace CodeDesignPlus.Net.Microservice.AsyncWorker.Consumers;

[QueueName("orderaggregate", "write_logger")]
public class CancelOrderHandler(ILogger<CancelOrderHandler> logger) : IEventHandler<OrderCancelledDomainEvent>
{
    private readonly ILogger<CancelOrderHandler> logger = logger;

    public Task HandleAsync(OrderCancelledDomainEvent data, CancellationToken token)
    {
        logger.LogInformation("OrderCancelledDomainEvent Recived, {aggregateId}, {json}", data.AggregateId, JsonConvert.SerializeObject(data));

        return Task.CompletedTask;
    }
}
