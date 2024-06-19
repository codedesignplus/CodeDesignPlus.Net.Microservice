using CodeDesignPlus.Net.Generator;
using MediatR;

namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.CompleteOrder
{
    public record CompleteOrderCommand(Guid Id) : IRequest;
}
