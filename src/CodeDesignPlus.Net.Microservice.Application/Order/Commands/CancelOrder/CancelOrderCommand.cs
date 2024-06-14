using CodeDesignPlus.Net.Generator;
using MediatR;

namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.CancelOrder
{
    [DtoGenerator]
    public class CancelOrderCommand : IRequest
    {
        public Guid Id { get; set; }
        public required string Reason { get; set; }
    }
}
