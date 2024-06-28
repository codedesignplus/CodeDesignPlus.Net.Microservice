namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.UpdateQuantityProduct;

public class UpdateQuantityProductCommandHandler(IOrderRepository orderRepository, IMessage message) : IRequestHandler<UpdateQuantityProductCommand>
{    public async Task Handle(UpdateQuantityProductCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.FindAsync(request.Id, cancellationToken);

        ApplicationGuard.IsNull(order, Errors.OrderNotFound);

        order.UpdateProductQuantity(request.ProductId, request.Quantity);

        await orderRepository.UpdateQuantityProductAsync(request.Id, request.ProductId, request.Quantity, cancellationToken);

        await message.PublishAsync(order.GetAndClearEvents(), cancellationToken);
    }
}
