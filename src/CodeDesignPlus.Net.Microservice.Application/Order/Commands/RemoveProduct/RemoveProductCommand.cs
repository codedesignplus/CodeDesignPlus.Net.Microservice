using CodeDesignPlus.Net.Generator;
using MediatR;

namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.RemoveProduct
{
    [DtoGenerator]
    public class RemoveProductCommand : IRequest
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
    }
}
