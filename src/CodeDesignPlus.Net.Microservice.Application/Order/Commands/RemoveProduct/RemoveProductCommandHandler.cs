namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.RemoveProduct;

public class RemoveProductCommandHandler(IOrderRepository orderRepository, IMessage message) : IRequestHandler<RemoveProductCommand>
{
    public async Task Handle(RemoveProductCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.FindAsync(request.Id, cancellationToken);

        ApplicationGuard.IsNull(order, Errors.OrderNotFound);

        order.RemoveProduct(request.ProductId);

        await orderRepository.RemoveProductFromOrderAsync(request.Id, request.ProductId, cancellationToken);

        await message.PublishAsync(order.GetAndClearEvents(), cancellationToken);
    }
}
