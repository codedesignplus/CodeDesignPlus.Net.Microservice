using MediatR;

namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.CancelOrder
{
    public class CancelOrderCommand : IRequest
    {
        public Guid Id { get; set; }
        public required string Reason { get; set; }
    }
}
