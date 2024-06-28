namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.AddProductToOrder;

public class AddProductToOrderCommandHandler(IOrderRepository orderRepository, IMessage message) : IRequestHandler<AddProductToOrderCommand>
{
    public async Task Handle(AddProductToOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.FindAsync(request.Id, cancellationToken);

        ApplicationGuard.IsNull(order, Errors.OrderNotFound);

        order.AddProduct(request.IdProduct, request.Name, request.Description, request.Price, request.Quantity);

        await orderRepository.AddProductToOrderAsync(request.Id, request.IdProduct, request.Name, request.Description, request.Price, request.Quantity, cancellationToken);

        await message.PublishAsync(order.GetAndClearEvents(), cancellationToken);
    }
}