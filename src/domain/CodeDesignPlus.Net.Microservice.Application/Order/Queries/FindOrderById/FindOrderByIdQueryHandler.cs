using CodeDesignPlus.Net.Cache.Abstractions;

namespace CodeDesignPlus.Net.Microservice.Application.Order.Queries.FindOrderById;

public class FindOrderByIdQueryHandler(IOrderRepository orderRepository, IMapper mapper, ICacheManager cacheManager, IUserContext user) : IRequestHandler<FindOrderByIdQuery, OrderDto>
{
    public async Task<OrderDto> Handle(FindOrderByIdQuery request, CancellationToken cancellationToken)
    {
        if(await cacheManager.ExistsAsync(request.Id.ToString())) 
            return await cacheManager.GetAsync<OrderDto>(request.Id.ToString());

        var result = await orderRepository.FindAsync<OrderAggregate>(request.Id, user.Tenant, cancellationToken);

        var order = mapper.Map<OrderDto>(result);

        await cacheManager.SetAsync(request.Id.ToString(), order);

        return order;
    }
}
