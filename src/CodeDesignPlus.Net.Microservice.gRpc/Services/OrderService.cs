
namespace CodeDesignPlus.Net.Microservice.gRpc.Services;

public class OrdersService(IMediator mediator, IMapper mapper) : Orders.OrdersBase
{
    public override async Task<GetOrdersResponse> GetOrders(GetOrdersRequest request, ServerCallContext context)
    {
        var criteria = mapper.Map<Net.Core.Abstractions.Models.Criteria.Criteria>(request.Criteria);

        var orders = await mediator.Send(new GetAllOrdersQuery(criteria), context.CancellationToken);

        return new GetOrdersResponse()
        {
            Orders = {
                orders.Select(x=> {
                    var order = mapper.Map<Order>(x);

                    order.Products.AddRange( x.Products.Select( p => mapper.Map<Product>(p)) );

                    return order;
                })
            }
        };
    }

    public override async Task<GetOrderResponse> GetOrder(GetOrderRequest request, ServerCallContext context)
    {
        if (Guid.TryParse(request.Id, out Guid id))
        {
            var result = await mediator.Send(new FindOrderByIdQuery(id), context.CancellationToken);

            var response = new GetOrderResponse()
            {
                Order = mapper.Map<OrderDto, Order>(result)
            };

            response.Order.Products.AddRange(result.Products.Select(x => mapper.Map<ProductDto, Product>(x)));

            return response;
        }

        throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Id"));
    }

    public override async Task<Empty> CreateOrder(CreateOrderRequest request, ServerCallContext context)
    {
        await mediator.Send(mapper.Map<CreateOrderCommand>(request), context.CancellationToken);

        return new Empty();
    }

    public override async Task<Empty> CancelOrder(CancelOrderRequest request, ServerCallContext context)
    {
        await mediator.Send(mapper.Map<CancelOrderCommand>(request), context.CancellationToken);

        return new Empty();
    }

    public override async Task<Empty> CompleteOrder(CompleteOrderRequest request, ServerCallContext context)
    {

        if (Guid.TryParse(request.Id, out Guid id))
        {
            await mediator.Send(new CompleteOrderCommand(id), context.CancellationToken);

            return new Empty();
        }

        throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Id"));
    }

    public override async Task<Empty> AddProductToOrder(AddProductToOrderRequest request, ServerCallContext context)
    {
        await mediator.Send(mapper.Map<AddProductToOrderCommand>(request), context.CancellationToken);

        return new Empty();
    }

    public override async Task<Empty> UpdateQuantityProductToOrder(UpdateQuantityProductToOrderRequest request, ServerCallContext context)
    {
        await mediator.Send(mapper.Map<UpdateQuantityProductCommand>(request), context.CancellationToken);

        return new Empty();
    }

    public override async Task<Empty> RemoveProductFromOrder(RemoveProductFromOrderRequest request, ServerCallContext context)
    {
        await mediator.Send(mapper.Map<RemoveProductCommand>(request), context.CancellationToken);

        return new Empty();
    }
}
