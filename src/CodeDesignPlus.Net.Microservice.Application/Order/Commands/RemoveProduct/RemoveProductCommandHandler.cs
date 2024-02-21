using CodeDesignPlus.Net.Microservice.Domain.Repositories;
using CodeDesignPlus.Net.PubSub.Abstractions;
using MediatR;

namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.RemoveProduct
{
    public class RemoveProductCommandHandler(IOrderRepository orderRepository, IMessage message) : IRequestHandler<RemoveProductCommand>
    {
        private readonly IOrderRepository orderRepository = orderRepository;
        private readonly IMessage message = message;

        public async Task Handle(RemoveProductCommand request, CancellationToken cancellationToken)
        {
            var order = await this.orderRepository.FindAsync(request.Id, cancellationToken);

            if (order is null)
                throw new InvalidOperationException("The order was not found.");

            order.RemoveProduct(request.ProductId);

            await this.orderRepository.RemoveProductFromOrderAsync(request.Id, request.ProductId, cancellationToken);

            await this.message.PublishAsync(order.GetAndClearEvents(), cancellationToken);
        }
    }
}
