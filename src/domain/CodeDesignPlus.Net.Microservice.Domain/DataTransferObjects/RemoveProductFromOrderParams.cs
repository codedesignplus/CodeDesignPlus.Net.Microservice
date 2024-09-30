using System;

namespace CodeDesignPlus.Net.Microservice.Domain.DataTransferObjects;

public class RemoveProductFromOrderParams: IDtoBase
{
    public required Guid Id { get; set; }
    public required Guid IdProduct { get; set; }
    public required long? UpdatedAt { get; set; }
    public required Guid? UpdateBy { get; set; }
}