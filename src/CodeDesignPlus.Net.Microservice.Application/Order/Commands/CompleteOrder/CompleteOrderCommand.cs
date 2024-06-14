using CodeDesignPlus.Net.Generator;
using MediatR;

namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.CompleteOrder
{
    [DtoGenerator]
    public class CompleteOrderCommand : IRequest
    {
        public Guid Id { get; set; }
        public DateTime CompleteAt { get; set; }
    }
}
