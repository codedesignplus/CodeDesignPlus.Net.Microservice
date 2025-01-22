namespace CodeDesignPlus.Net.Microservice.Application.Order.Queries.GetAllOrders;

public class GetAllOrdersQueryHandler(IOrderRepository orderRepository, IMapper mapper, IUserContext user)
    : IRequestHandler<GetAllOrdersQuery, List<OrderDto>>
{

    private readonly IOrderRepository orderRepository = orderRepository;
    private readonly IMapper mapper = mapper;

    public async Task<List<OrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        var result = await this.orderRepository.MatchingAsync<OrderAggregate>(request.Criteria, user.Tenant, cancellationToken).ConfigureAwait(false);

        var data = this.mapper.Map<List<OrderDto>>(result);

        return data;
    }
}
