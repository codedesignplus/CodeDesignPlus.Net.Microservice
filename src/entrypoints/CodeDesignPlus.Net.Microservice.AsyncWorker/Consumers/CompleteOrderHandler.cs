namespace CodeDesignPlus.Net.Microservice.AsyncWorker.Consumers;

[QueueName("orderaggregate", "complete_order")]
public class CompleteOrderHandler(ILogger<CompleteOrderHandler> logger) : IEventHandler<OrderCompletedDomainEvent>
{
    private readonly ILogger<CompleteOrderHandler> logger = logger;

    public Task HandleAsync(OrderCompletedDomainEvent data, CancellationToken token)
    {
        logger.LogInformation("OrderCompletedDomainEvent Recived, {AggregateId}, {Json}", data.AggregateId, JsonConvert.SerializeObject(data));

        return Task.CompletedTask;
    }
}
