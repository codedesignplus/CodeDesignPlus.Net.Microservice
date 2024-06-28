namespace CodeDesignPlus.Net.Microservice.gRpc.Core.Mapster;

public static class MapsterConfig
{
    public static void Configure()
    {
        TypeAdapterConfig<Net.Core.Abstractions.Models.Criteria.Criteria, Net.Core.Abstractions.Models.Criteria.Criteria>.NewConfig().TwoWays();
        TypeAdapterConfig<OrderDto, Order>.NewConfig().TwoWays();

        TypeAdapterConfig<ClientDto, Client>.NewConfig().TwoWays();
        TypeAdapterConfig<ProductDto, Product>.NewConfig().TwoWays();

        TypeAdapterConfig<AddProductToOrderRequest, AddProductToOrderCommand>.NewConfig().TwoWays();
        TypeAdapterConfig<CancelOrderRequest, CancelOrderCommand>.NewConfig().TwoWays();
        TypeAdapterConfig<CreateOrderRequest, CreateOrderCommand>.NewConfig().TwoWays();
        TypeAdapterConfig<UpdateQuantityProductToOrderRequest, UpdateQuantityProductCommand>.NewConfig().TwoWays();
        TypeAdapterConfig<RemoveProductFromOrderRequest, RemoveProductCommand>.NewConfig().TwoWays();
    }
}