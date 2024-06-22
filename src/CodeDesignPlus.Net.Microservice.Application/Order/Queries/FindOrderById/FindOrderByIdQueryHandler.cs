namespace CodeDesignPlus.Net.Microservice.Application.Order.Queries.FindOrderById;

public class FindOrderByIdQueryHandler(IOrderRepository orderRepository, IMapper mapper) : IRequestHandler<FindOrderByIdQuery, OrderDto>
{
    private readonly IOrderRepository orderRepository = orderRepository;
    private readonly IMapper mapper = mapper;

    public async Task<OrderDto> Handle(FindOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await this.orderRepository.FindAsync(request.Id, cancellationToken);

        return this.mapper.Map<OrderDto>(result);
    }
}
