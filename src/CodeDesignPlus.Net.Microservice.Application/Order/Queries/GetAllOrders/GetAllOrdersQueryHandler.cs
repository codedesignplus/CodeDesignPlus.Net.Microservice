using CodeDesignPlus.Net.Microservice.Application.Order.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Domain.Repositories;
using MapsterMapper;
using MediatR;

namespace CodeDesignPlus.Net.Microservice.Application.Order.Queries.GetAllOrders
{
    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, List<OrderDto>>
    {

        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;

        public GetAllOrdersQueryHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
        }

        public async Task<List<OrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var result = await this.orderRepository.GetAllOrdersAsync(cancellationToken);

            return this.mapper.Map<List<OrderDto>>(result);
        }
    }
}
