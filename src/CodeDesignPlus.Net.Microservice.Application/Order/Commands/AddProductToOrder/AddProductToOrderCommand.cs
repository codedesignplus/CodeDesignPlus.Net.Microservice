using CodeDesignPlus.Net.Generator;
using CodeDesignPlus.Net.Microservice.Application.Order.DataTransferObjects;
using MediatR;

namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.AddProductToOrder
{
    [DtoGenerator]
    public record AddProductToOrderCommand(Guid Id, ProductDto Product, int Quantity) : IRequest;
}
