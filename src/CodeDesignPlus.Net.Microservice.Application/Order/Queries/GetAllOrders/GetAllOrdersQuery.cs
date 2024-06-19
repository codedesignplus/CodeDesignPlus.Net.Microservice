using C = CodeDesignPlus.Net.Core.Abstractions.Models.Criteria;
using CodeDesignPlus.Net.Microservice.Application.Order.DataTransferObjects;
using MediatR;

namespace CodeDesignPlus.Net.Microservice.Application.Order.Queries.GetAllOrders
{
    public record GetAllOrdersQuery(C.Criteria Criteria) : IRequest<List<OrderDto>>;
}
