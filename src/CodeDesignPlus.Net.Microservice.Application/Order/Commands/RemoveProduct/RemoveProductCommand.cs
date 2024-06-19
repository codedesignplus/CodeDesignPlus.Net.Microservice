using CodeDesignPlus.Net.Generator;
using MediatR;

namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.RemoveProduct
{
    public record RemoveProductCommand(Guid Id, Guid ProductId) : IRequest;
}
