namespace CodeDesignPlus.Net.Microservice.Application.Order.Queries.FindOrderById;

public record FindOrderByIdQuery(Guid Id) : IRequest<OrderDto>;

