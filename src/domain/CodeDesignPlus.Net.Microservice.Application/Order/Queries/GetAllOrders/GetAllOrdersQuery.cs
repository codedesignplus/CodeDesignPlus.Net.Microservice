using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;

namespace CodeDesignPlus.Net.Microservice.Application.Order.Queries.GetAllOrders;

public record GetAllOrdersQuery(C.Criteria Criteria) : IRequest<Pagination<OrderDto>>;
