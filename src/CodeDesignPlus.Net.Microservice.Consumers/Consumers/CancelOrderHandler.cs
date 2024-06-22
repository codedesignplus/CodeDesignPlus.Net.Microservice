namespace CodeDesignPlus.Net.Microservice.AsyncWorker.Consumers;

public class CancelOrderHandler : IEventHandler<OrderCancelledDomainEvent>
{
    private readonly ILogger<CancelOrderHandler> logger;

    public CancelOrderHandler(ILogger<CancelOrderHandler> logger)
    {
        this.logger = logger;
    }

    public Task HandleAsync(OrderCancelledDomainEvent data, CancellationToken token)
    {
        logger.LogInformation("OrderCancelledDomainEvent Recived, {aggregateId}, {json}", data.AggregateId, JsonConvert.SerializeObject(data));

        return Task.CompletedTask;
    }
}
