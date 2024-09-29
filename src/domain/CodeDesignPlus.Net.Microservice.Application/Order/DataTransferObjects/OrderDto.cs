namespace CodeDesignPlus.Net.Microservice.Application.Order.DataTransferObjects;

public class OrderDto: IDtoBase
{
    public Guid Id { get; set; }
    public long? CompletedAt { get; set; }
    public long? CancelledAt { get; set; }
    public ClientDto Client { get; set; } = default!;
    public List<ProductDto> Products { get; set; } = [];
    public OrderStatus Status { get; set; }
    public string? ReasonForCancellation { get; set; }
    public long CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public long UpdatedAt { get; set; }
    public Guid UpdatedBy { get; set; }
    public bool IsActive { get; set; }
}
