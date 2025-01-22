using NodaTime;
using NodaTime.Serialization.Protobuf;

namespace CodeDesignPlus.Net.Microservice.gRpc.Core.Mapster;

public static class MapsterConfig
{
    public static void Configure()
    {
        TypeAdapterConfig<Net.Core.Abstractions.Models.Criteria.Criteria, Net.Core.Abstractions.Models.Criteria.Criteria>.NewConfig().TwoWays();
        TypeAdapterConfig<OrderDto, Order>
            .NewConfig()
            .Map(dest => dest.CreatedAt, src => NodaExtensions.ToTimestamp(src.CreatedAt))
            .Map(dest => dest.UpdatedAt, src => NodaExtensions.ToTimestamp(src.UpdatedAt ?? Instant.MinValue))
            .Map(dest => dest.CompletedAt, src => NodaExtensions.ToTimestamp(src.CompletedAt ?? Instant.MinValue))
            .Map(dest => dest.CancelledAt, src => NodaExtensions.ToTimestamp(src.CancelledAt ?? Instant.MinValue));

        TypeAdapterConfig<ClientDto, Client>.NewConfig().TwoWays();
        TypeAdapterConfig<ProductDto, Product>.NewConfig().TwoWays();
    }
}