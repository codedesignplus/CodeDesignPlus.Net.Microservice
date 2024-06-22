namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.CancelOrder;

public class CancelOrderCommandHandler(IOrderRepository orderRepository, IMessage message) : IRequestHandler<CancelOrderCommand>
{
    public async Task Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.FindAsync(request.Id, cancellationToken);

        ApplicationGuard.IsNull(order, Errors.OrderNotFound);

        order.CancelOrder(request.Reason);

        await orderRepository.CancelOrderAsync(request.Id, request.Reason, cancellationToken);

        await message.PublishAsync(order.GetAndClearEvents(), cancellationToken);
    }
}
