namespace CodeDesignPlus.Net.Microservice.Application.Order.Queries.GetAllOrders;

public class GetAllOrdersQueryHandler(IOrderRepository orderRepository, IMapper mapper, IUserContext user)
    : IRequestHandler<GetAllOrdersQuery, List<OrderDto>>
{

    public async Task<List<OrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        var result = await orderRepository.MatchingAsync<OrderAggregate>(request.Criteria, user.Tenant, cancellationToken);

        var data = mapper.Map<List<OrderDto>>(result);

        return data;
    }
}
