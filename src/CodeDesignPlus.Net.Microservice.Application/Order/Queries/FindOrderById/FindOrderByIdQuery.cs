using CodeDesignPlus.Net.Microservice.Application.Order.DataTransferObjects;
using MediatR;

namespace CodeDesignPlus.Net.Microservice.Application.Order.Queries.FindOrderById
{
    public class FindOrderByIdQuery : IRequest<OrderDto>
    {
        public Guid Id { get; set; }
    }
}
