using System;

namespace CodeDesignPlus.Net.Microservice.Domain.DataTransferObjects;

public class CompleteOrderParams: IDtoBase
{
    public required Guid Id { get; set; }
    public required long? CompletedAt { get; set; }
    public required OrderStatus OrderStatus { get; set; }
    public required long? UpdatedAt { get; set; }
    public required Guid? UpdateBy { get; set; }
}