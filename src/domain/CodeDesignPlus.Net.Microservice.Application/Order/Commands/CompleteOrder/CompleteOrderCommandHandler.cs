namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.CompleteOrder;

public class CompleteOrderCommandHandler(IOrderRepository orderRepository, IUserContext user, IPubSub pubsub) : IRequestHandler<CompleteOrderCommand>
{
    public async Task Handle(CompleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.FindAsync<OrderAggregate>(request.Id, user.Tenant, cancellationToken);

        ApplicationGuard.IsNull(order, Errors.OrderNotFound);

        order.CompleteOrder(user.IdUser);

        await orderRepository.CompleteOrderAsync(new CompleteOrderParams()
        {
            Id = order.Id,
            CompletedAt = order.CompletedAt,
            OrderStatus = order.Status,
            UpdatedAt = order.UpdatedAt,
            UpdateBy = order.UpdatedBy
        }, user.Tenant, cancellationToken);

        await pubsub.PublishAsync(order.GetAndClearEvents(), cancellationToken);
    }
}

