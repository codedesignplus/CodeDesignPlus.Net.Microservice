using CodeDesignPlus.Net.Microservice.Domain.ValueObjects;

namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.CreateOrder;

public class CreateOrderCommandHandler(IOrderRepository orderRepository, IUserContext user, IPubSub pubsub) : IRequestHandler<CreateOrderCommand>
{
    public async Task Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var exist = await orderRepository.ExistsAsync<OrderAggregate>(request.Id, user.Tenant, cancellationToken);

        ApplicationGuard.IsTrue(exist, Errors.OrderAlreadyExists);

        var client = ClientValueObject.Create(request.Client.Id, request.Client.Name, request.Client.Document, request.Client.TypeDocument);

        var address = AddressValueObject.Create(request.Address.Country, request.Address.State, request.Address.City, request.Address.Address, request.Address.CodePostal);

        var order = OrderAggregate.Create(request.Id, client, address, user.Tenant, user.IdUser);

        await orderRepository.CreateAsync(order, cancellationToken);

        await pubsub.PublishAsync(order.GetAndClearEvents(), cancellationToken);
    }
}
