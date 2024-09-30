using System;

namespace CodeDesignPlus.Net.Microservice.Domain.DataTransferObjects;

public class UpdateQuantityProductParams: IDtoBase
{
    public required Guid Id { get; set; }
    public required Guid ProductId { get; set; }
    public required int NewQuantity { get; set; }
    public required long? UpdatedAt { get; set; }
    public required Guid? UpdateBy { get; set; }
}