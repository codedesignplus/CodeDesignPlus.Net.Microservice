using CodeDesignPlus.Net.Microservice.Domain.Repositories;
using CodeDesignPlus.Net.PubSub.Abstractions;
using MediatR;

namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.CompleteOrder
{
    public class CompleteOrderCommandHandler(IOrderRepository orderRepository, IPubSub message) : IRequestHandler<CompleteOrderCommand>
    {
        private readonly IOrderRepository orderRepository = orderRepository;
        private readonly IPubSub message = message;

        public async Task Handle(CompleteOrderCommand request, CancellationToken cancellationToken)
        {

            var order = await this.orderRepository.FindAsync(request.Id, cancellationToken);

            if (order is null)
                throw new InvalidOperationException("The order was not found.");

            order.CompleteOrder();

            await this.orderRepository.CompleteOrderAsync(request.Id, request.CompleteAt, cancellationToken);

            await this.message.PublishAsync(order.GetAndClearEvents(), cancellationToken);
        }
    }
}
