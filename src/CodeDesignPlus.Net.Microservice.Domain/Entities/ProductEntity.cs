using CodeDesignPlus.Net.Core.Abstractions;

namespace CodeDesignPlus.Net.Microservice.Domain.Entities
{
    public class ProductEntity : IEntity, IAuditTrail
    {
        public Guid Id { get; set; }
        public Guid Tenant { get; set; }
        public bool IsActive { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public Guid IdUserCreator { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Quantity { get; set; }
    }
}
