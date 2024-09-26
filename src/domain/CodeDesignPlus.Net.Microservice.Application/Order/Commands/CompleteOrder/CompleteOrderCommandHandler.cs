namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.CompleteOrder;

public class CompleteOrderCommandHandler(IOrderRepository orderRepository,IUserContext user,  IMessage message) : IRequestHandler<CompleteOrderCommand>
{
    public async Task Handle(CompleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.FindAsync(request.Id, cancellationToken);

        ApplicationGuard.IsNull(order, Errors.OrderNotFound);

        order.CompleteOrder(user.IdUser);

        await orderRepository.CompleteOrderAsync(order.Id, order.CompletedAt, order.Status, order.UpdatedAt, order.UpdatedBy, cancellationToken);

        await message.PublishAsync(order.GetAndClearEvents(), cancellationToken);
    }
}

