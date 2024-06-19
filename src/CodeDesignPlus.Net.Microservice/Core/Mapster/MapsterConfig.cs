using CodeDesignPlus.Net.Microservice.Application.Order.Commands.AddProductToOrder;
using CodeDesignPlus.Net.Microservice.Application.Order.Commands.UpdateQuantityProduct;
using CodeDesignPlus.Microservice.Api.Dtos;
using CodeDesignPlus.Net.Microservice.Application.Order.Commands.CancelOrder;
using CodeDesignPlus.Net.Microservice.Application.Order.Commands.CompleteOrder;
using CodeDesignPlus.Net.Microservice.Application.Order.Commands.CreateOrder;
using CodeDesignPlus.Net.Microservice.Application.Order.Commands.RemoveProduct;
using Mapster;

namespace CodeDesignPlus.Net.Microservice;

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