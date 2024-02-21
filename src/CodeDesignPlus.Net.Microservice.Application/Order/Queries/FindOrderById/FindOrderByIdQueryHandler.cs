using CodeDesignPlus.Net.Microservice.Application.Order.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Domain.Repositories;
using MapsterMapper;
using MediatR;

namespace CodeDesignPlus.Net.Microservice.Application.Order.Queries.FindOrderById
{
    public class FindOrderByIdQueryHandler : IRequestHandler<FindOrderByIdQuery, OrderDto>
    {

        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;

        public FindOrderByIdQueryHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
        }

        public async Task<OrderDto> Handle(FindOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await this.orderRepository.FindAsync(request.Id, cancellationToken);

            return this.mapper.Map<OrderDto>(result);
        }
    }
}
