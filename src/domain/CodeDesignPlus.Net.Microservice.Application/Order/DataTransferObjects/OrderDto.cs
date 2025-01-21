namespace CodeDesignPlus.Net.Microservice.Application.Order.DataTransferObjects;

public class OrderDto: IDtoBase
{
    public Guid Id { get; set; }
    public Instant? CompletedAt { get; set; }
    public Instant? CancelledAt { get; set; }
    public ClientDto Client { get; set; } = default!;
    public AddressDto ShippingAddress { get; set; } = default!;
    public List<ProductDto> Products { get; set; } = [];
    public OrderStatus Status { get; set; }
    public string? ReasonForCancellation { get; set; }
    public Instant CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public Instant? UpdatedAt { get; set; }
    public Guid UpdatedBy { get; set; }
    public bool IsActive { get; set; }
}
