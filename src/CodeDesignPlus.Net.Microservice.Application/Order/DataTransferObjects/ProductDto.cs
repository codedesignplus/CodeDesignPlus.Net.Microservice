namespace CodeDesignPlus.Net.Microservice.Application.Order.DataTransferObjects;

public class ProductDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
