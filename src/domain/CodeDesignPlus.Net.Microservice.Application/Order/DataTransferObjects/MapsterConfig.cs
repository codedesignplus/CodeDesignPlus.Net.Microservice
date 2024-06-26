﻿namespace CodeDesignPlus.Net.Microservice.Application.Order.DataTransferObjects;

public static class MapsterConfig
{
    public static void Configure()
    {
        TypeAdapterConfig<ClientDto, ClientEntity>.NewConfig().TwoWays();
        TypeAdapterConfig<ProductDto, ProductDto>.NewConfig().TwoWays();

        TypeAdapterConfig<OrderAggregate, OrderDto>.NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Client, src => src.Client)
            .Map(dest => dest.Products, src => src.Products)
            .Map(dest => dest.CompletedAt, src => src.CompletedAt)
            .Map(dest => dest.CancelledAt, src => src.CancelledAt)
            .Map(dest => dest.Status, src => src.Status)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt)
            .Map(dest => dest.ReasonForCancellation, src => src.ReasonForCancellation);
    }
}
