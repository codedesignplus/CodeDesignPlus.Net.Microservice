using CodeDesignPlus.Net.Microservice.Domain;
using CodeDesignPlus.Net.Microservice.Domain.Entities;
using CodeDesignPlus.Net.Microservice.Domain.Repositories;
using CodeDesignPlus.Net.PubSub.Abstractions;
using MapsterMapper;
using MediatR;

namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.CreateOrder
{
    public class CreateOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, IPubSub pubSub) : IRequestHandler<CreateOrderCommand>
    {
        private readonly IOrderRepository orderRepository = orderRepository;
        private readonly IMapper mapper = mapper;
        private readonly IPubSub message = pubSub;

        public async Task Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await this.orderRepository.FindAsync(request.Id, cancellationToken);

            if (order is not null)
                throw new InvalidOperationException("The order already exists.");

            order = OrderAggregate.Create(request.Id, this.mapper.Map<ClientEntity>(request.Client), request.Tenant);

            await this.orderRepository.CreateOrderAsync(order.Id, order.Client, order.Tenant, cancellationToken);

            await this.message.PublishAsync(order.GetAndClearEvents(), cancellationToken);
        }
    }
}
