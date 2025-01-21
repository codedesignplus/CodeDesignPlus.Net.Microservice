using System;

namespace CodeDesignPlus.Net.Microservice.Domain.DataTransferObjects;

public class AddProductToOrderParams: IDtoBase
{
    public required Guid Id { get; set; }
    public required Guid IdProduct { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required long Price { get; set; }
    public required int Quantity { get; set; }
    public required Instant? UpdatedAt { get; set; }
    public required Guid? UpdateBy { get; set; }
}