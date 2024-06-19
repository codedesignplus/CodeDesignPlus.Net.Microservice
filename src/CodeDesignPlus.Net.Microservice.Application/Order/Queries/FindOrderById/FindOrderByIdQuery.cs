using CodeDesignPlus.Net.Microservice.Application.Order.DataTransferObjects;
using MediatR;

namespace CodeDesignPlus.Net.Microservice.Application.Order.Queries.FindOrderById
{
    public record FindOrderByIdQuery(Guid Id) : IRequest<OrderDto>;
}
