namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.AddProductToOrder;

public class AddProductToOrderCommandHandler(IOrderRepository orderRepository, IUserContext user, IMessage message) : IRequestHandler<AddProductToOrderCommand>
{
    public async Task Handle(AddProductToOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.FindAsync(request.Id, cancellationToken);

        ApplicationGuard.IsNull(order, Errors.OrderNotFound);

        order.AddProduct(request.IdProduct, request.Name, request.Description, request.Price, request.Quantity, user.IdUser);

        await orderRepository.AddProductToOrderAsync(new AddProductToOrderParams()
        {
            Id = order.Id,
            IdProduct = request.IdProduct,
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Quantity = request.Quantity,
            UpdatedAt = order.UpdatedAt,
            UpdateBy = user.IdUser
        }, cancellationToken);

        await message.PublishAsync(order.GetAndClearEvents(), cancellationToken);
    }
}