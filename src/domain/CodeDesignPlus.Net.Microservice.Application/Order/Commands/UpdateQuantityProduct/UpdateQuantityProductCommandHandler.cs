namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.UpdateQuantityProduct;

public class UpdateQuantityProductCommandHandler(IOrderRepository orderRepository, IUserContext user, IPubSub pubsub) : IRequestHandler<UpdateQuantityProductCommand>
{
    public async Task Handle(UpdateQuantityProductCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.FindAsync<OrderAggregate>(request.Id, user.Tenant, cancellationToken);

        ApplicationGuard.IsNull(order, Errors.OrderNotFound);

        order.UpdateProductQuantity(request.ProductId, request.Quantity, user.IdUser);

        await orderRepository.UpdateQuantityProductAsync(order.Id, user.Tenant, new UpdateQuantityProductParams()
        {
            Id = request.ProductId,
            NewQuantity = request.Quantity,
            UpdatedAt = order.UpdatedAt,
            UpdateBy = user.IdUser
        }, cancellationToken);

        await pubsub.PublishAsync(order.GetAndClearEvents(), cancellationToken);
    }
}
