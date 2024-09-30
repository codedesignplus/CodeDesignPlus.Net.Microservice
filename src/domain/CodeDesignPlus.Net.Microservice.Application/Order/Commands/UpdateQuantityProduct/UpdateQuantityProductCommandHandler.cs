namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.UpdateQuantityProduct;

public class UpdateQuantityProductCommandHandler(IOrderRepository orderRepository, IUserContext user, IMessage message) : IRequestHandler<UpdateQuantityProductCommand>
{
    public async Task Handle(UpdateQuantityProductCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.FindAsync(request.Id, cancellationToken);

        ApplicationGuard.IsNull(order, Errors.OrderNotFound);

        order.UpdateProductQuantity(request.ProductId, request.Quantity, user.IdUser);

        await orderRepository.UpdateQuantityProductAsync(new UpdateQuantityProductParams()
        {
            Id = order.Id,
            ProductId = request.ProductId,
            NewQuantity = request.Quantity,
            UpdatedAt = order.UpdatedAt,
            UpdateBy = user.IdUser
        }, cancellationToken);

        await message.PublishAsync(order.GetAndClearEvents(), cancellationToken);
    }
}
