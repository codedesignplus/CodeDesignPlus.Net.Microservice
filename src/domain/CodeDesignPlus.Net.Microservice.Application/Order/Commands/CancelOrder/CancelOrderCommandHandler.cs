namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.CancelOrder;

public class CancelOrderCommandHandler(IOrderRepository orderRepository, IUserContext user, IMessage message) : IRequestHandler<CancelOrderCommand>
{
    public async Task Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.FindAsync(request.Id, cancellationToken);

        ApplicationGuard.IsNull(order, Errors.OrderNotFound);

        order.CancelOrder(request.Reason, user.IdUser);

        await orderRepository.CancelOrderAsync(order.Id, order.Status, order.ReasonForCancellation, order.CancelledAt, order.UpdatedAt, order.UpdatedBy, cancellationToken);

        await message.PublishAsync(order.GetAndClearEvents(), cancellationToken);
    }
}
