using CodeDesignPlus.Net.Microservice.Domain.ValueObjects;

namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.CreateOrder;

public class CreateOrderCommandHandler(IOrderRepository orderRepository, IUserContext user, IMessage message) : IRequestHandler<CreateOrderCommand>
{
    public async Task Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.FindAsync(request.Id, cancellationToken);

        ApplicationGuard.IsNotNull(order, Errors.OrderAlreadyExists);


        var client = ClientValueObject.Create(request.Client.Id, request.Client.Name, request.Client.Document, request.Client.TypeDocument);

        var address = AddressValueObject.Create(request.Address.Country, request.Address.State, request.Address.City, request.Address.Address, request.Address.CodePostal);

        order = OrderAggregate.Create(request.Id, client, address, user.Tenant, user.IdUser);

        await orderRepository.CreateOrderAsync(order, cancellationToken);

        await message.PublishAsync(order.GetAndClearEvents(), cancellationToken);
    }
}
