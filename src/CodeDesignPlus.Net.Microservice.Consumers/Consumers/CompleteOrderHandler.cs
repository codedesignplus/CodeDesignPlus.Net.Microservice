namespace CodeDesignPlus.Net.Microservice.AsyncWorker.Consumers;

public class CompleteOrderHandler : IEventHandler<OrderCompletedDomainEvent>
{
    private readonly ILogger<CompleteOrderHandler> logger;

    public CompleteOrderHandler(ILogger<CompleteOrderHandler> logger)
    {
        this.logger = logger;
    }

    public Task HandleAsync(OrderCompletedDomainEvent data, CancellationToken token)
    {
        logger.LogInformation("OrderCompletedDomainEvent Recived, {aggregateId}, {json}", data.AggregateId, JsonConvert.SerializeObject(data));

        return Task.CompletedTask;
    }
}
