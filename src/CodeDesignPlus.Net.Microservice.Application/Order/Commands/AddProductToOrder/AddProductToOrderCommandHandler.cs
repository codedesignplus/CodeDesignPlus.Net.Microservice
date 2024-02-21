using CodeDesignPlus.Net.Microservice.Domain.Entities;
using CodeDesignPlus.Net.Microservice.Domain.Repositories;
using CodeDesignPlus.Net.PubSub.Abstractions;
using MapsterMapper;
using MediatR;

namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.AddProductToOrder
{
    public class AddProductToOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, IMessage message) : IRequestHandler<AddProductToOrderCommand>
    {
        private readonly IOrderRepository orderRepository = orderRepository;
        private readonly IMapper mapper = mapper;
        private readonly IMessage message = message;

        public async Task Handle(AddProductToOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await this.orderRepository.FindAsync(request.Id, cancellationToken);

            if (order is null)
                throw new InvalidOperationException("The order was not found.");

            var product = this.mapper.Map<ProductEntity>(request.Product);

            order.AddProduct(product, request.Quantity);

            await this.orderRepository.AddProductToOrderAsync(request.Id, product, request.Quantity, cancellationToken);

            await this.message.PublishAsync(order.GetAndClearEvents(), cancellationToken);
        }
    }
}
