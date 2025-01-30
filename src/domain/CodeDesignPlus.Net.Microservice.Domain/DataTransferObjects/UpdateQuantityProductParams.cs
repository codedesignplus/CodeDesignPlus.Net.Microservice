using System;

namespace CodeDesignPlus.Net.Microservice.Domain.DataTransferObjects;

public class UpdateQuantityProductParams: IDtoBase
{
    public required Guid Id { get; set; }
    public required int NewQuantity { get; set; }
    public required Instant? UpdatedAt { get; set; }
    public required Guid? UpdateBy { get; set; }
}