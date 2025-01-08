using CodeDesignPlus.Net.Cache.Abstractions;

namespace CodeDesignPlus.Net.Microservice.Application.Order.Queries.FindOrderById;

public class FindOrderByIdQueryHandler(IOrderRepository orderRepository, IMapper mapper, ICacheManager cacheManager) : IRequestHandler<FindOrderByIdQuery, OrderDto>
{
    private readonly IOrderRepository orderRepository = orderRepository;
    private readonly IMapper mapper = mapper;

    public async Task<OrderDto> Handle(FindOrderByIdQuery request, CancellationToken cancellationToken)
    {
        if(await cacheManager.ExistsAsync(request.Id.ToString())) 
            return await cacheManager.GetAsync<OrderDto>(request.Id.ToString());

        var result = await this.orderRepository.FindAsync(request.Id, cancellationToken);

        var order = this.mapper.Map<OrderDto>(result);

        await cacheManager.SetAsync(request.Id.ToString(), order);

        return order;
    }
}
