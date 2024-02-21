using MediatR;

namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.CompleteOrder
{
    public class CompleteOrderCommand : IRequest
    {
        public Guid Id { get; set; }
        public DateTime CompleteAt { get; set; }
    }
}
