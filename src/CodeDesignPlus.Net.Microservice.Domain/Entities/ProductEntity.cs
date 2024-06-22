namespace CodeDesignPlus.Net.Microservice.Domain.Entities;

public class ProductEntity : IEntityBase
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public long Price { get; set; }
    public int Quantity { get; set; }
}
