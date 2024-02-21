using CodeDesignPlus.Net.Microservice.Application.Order.DataTransferObjects;
using MediatR;

namespace CodeDesignPlus.Net.Microservice.Application.Order.Queries.GetAllOrders
{
    public class GetAllOrdersQuery : IRequest<List<OrderDto>>
    {

    }
}
