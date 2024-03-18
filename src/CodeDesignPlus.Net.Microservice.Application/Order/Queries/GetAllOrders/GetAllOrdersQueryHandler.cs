using CodeDesignPlus.Net.Core.Abstractions.Options;
using CodeDesignPlus.Net.Microservice.Application.Order.DataTransferObjects;
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
            if (await this.redisService.Database.KeyExistsAsync("orders") )
            {
                var json = await this.redisService.Database.StringGetAsync("orders");

                return JsonConvert.DeserializeObject<List<OrderDto>>(json!)!;
            }

            var result = await this.orderRepository.GetAllOrdersAsync(cancellationToken);

            var data = this.mapper.Map<List<OrderDto>>(result);

            await this.redisService.Database.StringSetAsync("orders", JsonConvert.SerializeObject(data), TimeSpan.FromMinutes(10));

            return data;
        }
    }
}
