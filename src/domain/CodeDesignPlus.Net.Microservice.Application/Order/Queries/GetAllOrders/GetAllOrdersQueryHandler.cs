using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;

namespace CodeDesignPlus.Net.Microservice.Application.Order.Queries.GetAllOrders;

public class GetAllOrdersQueryHandler(IOrderRepository orderRepository, IMapper mapper, IUserContext user)
    : IRequestHandler<GetAllOrdersQuery, Pagination<OrderDto>>
{

    public async Task<Pagination<OrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        var result = await orderRepository.MatchingAsync<OrderAggregate>(request.Criteria, user.Tenant, cancellationToken);

        var data = mapper.Map<Pagination<OrderDto>>(result);

        return data;
    }
}
