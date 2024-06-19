using CodeDesignPlus.Net.Core.Abstractions;

namespace CodeDesignPlus.Net.Microservice.Domain.Entities
{
    public class ProductEntity : IEntity
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public long Price { get; set; }
        public int Quantity { get; set; }
    }
}
