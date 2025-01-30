using System;

namespace CodeDesignPlus.Net.Microservice.Domain.DataTransferObjects;

public class CompleteOrderParams: IDtoBase
{
    public required Guid Id { get; set; }
    public required Instant? CompletedAt { get; set; }
    public required OrderStatus OrderStatus { get; set; }
    public required Instant? UpdatedAt { get; set; }
    public required Guid? UpdateBy { get; set; }
}