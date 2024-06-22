namespace CodeDesignPlus.Net.Microservice.Rest.Core.Mapster;

public static class MapsterConfig
{
    public static void Configure()
    {
        TypeAdapterConfig<AddProductToOrderDto, AddProductToOrderCommand>.NewConfig().TwoWays();
        TypeAdapterConfig<CancelOrderDto, CancelOrderCommand>.NewConfig().TwoWays();
        TypeAdapterConfig<CreateOrderDto, CreateOrderCommand>.NewConfig().TwoWays();
        TypeAdapterConfig<UpdateQuantityProductDto, UpdateQuantityProductCommand>.NewConfig().TwoWays();
    }
}