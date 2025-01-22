namespace CodeDesignPlus.Net.Microservice.gRpc.Services;

public class OrderService(IMediator mediator, IMapper mapper) : Orders.OrdersBase
{
    public override async Task GetOrder(IAsyncStreamReader<GetOrderRequest> requestStream, IServerStreamWriter<GetOrderResponse> responseStream, ServerCallContext context)
    {
        await foreach (var request in requestStream.ReadAllAsync())
        {
            if (Guid.TryParse(request.Id, out Guid id))
            {
                var result = await mediator.Send(new FindOrderByIdQuery(id), context.CancellationToken);

                var response = new GetOrderResponse()
                {
                    Order = mapper.Map<OrderDto, Order>(result)
                };

                response.Order.Products.AddRange(result.Products.Select(x => mapper.Map<ProductDto, Product>(x)));

                await responseStream.WriteAsync(response);
            }
            else
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Id"));
            }
        }
    }
}
