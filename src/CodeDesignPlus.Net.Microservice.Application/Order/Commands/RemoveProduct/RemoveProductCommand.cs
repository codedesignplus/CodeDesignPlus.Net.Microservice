using MediatR;

namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.RemoveProduct
{
    public class RemoveProductCommand : IRequest
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
    }
}
