namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.CreateOrder;

public class CreateOrderCommandHandler(IOrderRepository orderRepository, IUserContext user, IMessage message) : IRequestHandler<CreateOrderCommand>
{
    public async Task Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.FindAsync(request.Id, cancellationToken);

        ApplicationGuard.IsNotNull(order, Errors.OrderAlreadyExists);

        order = OrderAggregate.Create(request.Id, request.Client.Id, request.Client.Name, user.Tenant, user.IdUser);

        await orderRepository.CreateOrderAsync(order, cancellationToken);

        await message.PublishAsync(order.GetAndClearEvents(), cancellationToken);
    }
}
