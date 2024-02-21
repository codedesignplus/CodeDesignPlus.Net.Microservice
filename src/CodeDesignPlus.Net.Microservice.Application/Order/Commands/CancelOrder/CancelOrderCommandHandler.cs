using CodeDesignPlus.Net.Microservice.Domain.Repositories;
using CodeDesignPlus.Net.Microservice.Domain.ValueObjects;
using CodeDesignPlus.Net.PubSub.Abstractions;
using MediatR;

namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.CancelOrder
{
    public class CancelOrderCommandHandler(IOrderRepository orderRepository, IMessage message) : IRequestHandler<CancelOrderCommand>
    {
        private readonly IOrderRepository orderRepository = orderRepository;
        private readonly IMessage message = message;

        public async Task Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await this.orderRepository.FindAsync(request.Id, cancellationToken);

            if (order is null)
                throw new InvalidOperationException("The order was not found.");

            if (order.Status == OrderStatus.Cancelled)
                throw new InvalidOperationException("The order has already been canceled.");

            order.CancelOrder(request.Reason);

            await this.orderRepository.CancelOrderAsync(request.Id, request.Reason, cancellationToken);

            await this.message.PublishAsync(order.GetAndClearEvents(), cancellationToken);
        }
    }
}
