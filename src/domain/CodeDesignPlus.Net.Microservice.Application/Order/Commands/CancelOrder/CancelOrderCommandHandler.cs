namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.CancelOrder;

public class CancelOrderCommandHandler(IOrderRepository orderRepository, IUserContext user, IPubSub pubsub) : IRequestHandler<CancelOrderCommand>
{
    public async Task Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.FindAsync<OrderAggregate>(request.Id, user.Tenant, cancellationToken);

        ApplicationGuard.IsNull(order, Errors.OrderNotFound);

        order.CancelOrder(request.Reason, user.IdUser);

        await orderRepository.CancelOrderAsync(new CancelOrderParams()
        {
            Id = order.Id,
            OrderStatus = order.Status,
            Reason = order.ReasonForCancellation,
            CancelledAt = order.CancelledAt,
            UpdatedAt = order.UpdatedAt,
            UpdateBy = order.UpdatedBy
        }, user.Tenant, cancellationToken);

        await pubsub.PublishAsync(order.GetAndClearEvents(), cancellationToken);
    }
}
