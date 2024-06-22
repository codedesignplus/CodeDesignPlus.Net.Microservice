namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.CompleteOrder;

public class CompleteOrderCommandHandler(IOrderRepository orderRepository, IPubSub message) : IRequestHandler<CompleteOrderCommand>
{
    public async Task Handle(CompleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.FindAsync(request.Id, cancellationToken);

        ApplicationGuard.IsNull(order, Errors.OrderNotFound);

        order.CompleteOrder();

        await orderRepository.CompleteOrderAsync(request.Id, order.CompletedAt, cancellationToken);

        await message.PublishAsync(order.GetAndClearEvents(), cancellationToken);
    }
}

