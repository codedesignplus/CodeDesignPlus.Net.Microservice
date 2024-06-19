using CodeDesignPlus.Net.Microservice.Domain;
using CodeDesignPlus.Net.Microservice.Domain.Entities;
using Mapster;

namespace CodeDesignPlus.Net.Microservice.Application.Order.DataTransferObjects
{
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
                .Map(dest => dest.CompletionDate, src => src.CompleteAt)
                .Map(dest => dest.CancellationDate, src => src.CancelledOn)
                .Map(dest => dest.Status, src => src.Status)
                .Map(dest => dest.CreatedAt, src => src.CreatedAt)
                .Map(dest => dest.ReasonForCancellation, src => src.ReasonForCancellation);
        }
    }
}
