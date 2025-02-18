﻿namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.AddProductToOrder;

public class AddProductToOrderCommandHandler(IOrderRepository orderRepository, IUserContext user, IPubSub pubsub) : IRequestHandler<AddProductToOrderCommand>
{
    public async Task Handle(AddProductToOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.FindAsync<OrderAggregate>(request.Id, user.Tenant, cancellationToken);

        ApplicationGuard.IsNull(order, Errors.OrderNotFound);

        order.AddProduct(request.IdProduct, request.Name, request.Description, request.Price, request.Quantity, user.IdUser);

        await orderRepository.AddProductToOrderAsync(order.Id, user.Tenant, new AddProductToOrderParams()
        {
            Id = request.IdProduct,
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Quantity = request.Quantity,
            UpdatedAt = order.UpdatedAt,
            UpdateBy = user.IdUser
        }, cancellationToken);

        await pubsub.PublishAsync(order.GetAndClearEvents(), cancellationToken);
    }
}