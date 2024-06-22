namespace CodeDesignPlus.Net.Microservice.AsyncWorker.Consumers;

public class OrderCreatedHandler : IEventHandler<OrderCreatedDomainEvent>
{
    private readonly ILogger<OrderCreatedHandler> logger;

    public OrderCreatedHandler(ILogger<OrderCreatedHandler> logger)
    {
        this.logger = logger;
    }

    public Task HandleAsync(OrderCreatedDomainEvent data, CancellationToken token)
    {
        logger.LogInformation("OrderCreatedDomainEvent Recived, {aggregateId}, {json}", data.AggregateId, JsonConvert.SerializeObject(data));

        return Task.CompletedTask;
    }
}

