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
            .Map(dest => dest.CreatedAt, src => src.CreatedAt.ToTimestamp())
            .Map(dest => dest.UpdatedAt, src => (src.UpdatedAt ?? Instant.MinValue).ToTimestamp())
            .Map(dest => dest.CompletedAt, src => (src.CompletedAt ?? Instant.MinValue).ToTimestamp())
            .Map(dest => dest.CancelledAt, src => (src.CancelledAt ?? Instant.MinValue).ToTimestamp());

        TypeAdapterConfig<ClientDto, Client>.NewConfig().TwoWays();
        TypeAdapterConfig<ProductDto, Product>.NewConfig().TwoWays();
    }
}