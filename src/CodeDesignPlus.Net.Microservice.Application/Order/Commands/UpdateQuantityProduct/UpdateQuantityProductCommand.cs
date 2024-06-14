using CodeDesignPlus.Net.Generator;
using MediatR;

namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.UpdateQuantityProduct
{
    [DtoGenerator]
    public class UpdateQuantityProductCommand : IRequest
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
