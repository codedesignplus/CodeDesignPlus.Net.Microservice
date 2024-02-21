using CodeDesignPlus.Net.Microservice.Domain.Repositories;
using CodeDesignPlus.Net.PubSub.Abstractions;
using MediatR;

namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.UpdateQuantityProduct
{
    public class UpdateQuantityProductCommandHandler(IOrderRepository orderRepository, IMessage message) : IRequestHandler<UpdateQuantityProductCommand>
    {
        private readonly IOrderRepository orderRepository = orderRepository;
        private readonly IMessage message = message;

        public async Task Handle(UpdateQuantityProductCommand request, CancellationToken cancellationToken)
        {
            var order = await this.orderRepository.FindAsync(request.Id, cancellationToken);

            if (order is null)
                throw new InvalidOperationException("The order was not found.");

            order.UpdateProductQuantity(request.ProductId, request.Quantity);

            await this.orderRepository.UpdateQuantityProductAsync(request.Id, request.ProductId, request.Quantity, cancellationToken);

            await this.message.PublishAsync(order.GetAndClearEvents(), cancellationToken);
        }
    }
}
