using System;

namespace CodeDesignPlus.Net.Microservice.Domain.DataTransferObjects;

public class CancelOrderParams: IDtoBase
{
    public required Guid Id { get; set; }
    public required OrderStatus OrderStatus { get; set; }
    public required string? Reason { get; set; }
    public required long? CancelledAt { get; set; }
    public required long? UpdatedAt { get; set; }
    public required Guid? UpdateBy { get; set; }
}