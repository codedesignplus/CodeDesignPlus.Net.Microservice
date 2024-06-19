using CodeDesignPlus.Net.Core.Abstractions.Options;
using CodeDesignPlus.Net.Microservice.Application.Order.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Domain;
using CodeDesignPlus.Net.Microservice.Domain.Repositories;
using CodeDesignPlus.Net.Redis.Abstractions;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CodeDesignPlus.Net.Microservice.Application.Order.Queries.GetAllOrders
{
    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, List<OrderDto>>
    {

        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;
        private readonly IRedisService redisService;

        public GetAllOrdersQueryHandler(IOrderRepository orderRepository, IMapper mapper, IRedisServiceFactory redisServiceFactory)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
            this.redisService = redisServiceFactory.Create(FactoryConst.RedisService);
        }

        public async Task<List<OrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var result = await this.orderRepository.MatchingAsync<OrderAggregate>(request.Criteria, cancellationToken).ConfigureAwait(false);

            var data = this.mapper.Map<List<OrderDto>>(result);

            return data;
        }
    }
}
