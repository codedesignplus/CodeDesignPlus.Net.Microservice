namespace CodeDesignPlus.Net.Microservice.Application.Order.Queries.GetAllOrders;

public record GetAllOrdersQuery(C.Criteria Criteria) : IRequest<List<OrderDto>>;
