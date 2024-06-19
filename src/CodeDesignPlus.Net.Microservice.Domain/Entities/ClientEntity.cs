using CodeDesignPlus.Net.Core.Abstractions;

namespace CodeDesignPlus.Net.Microservice.Domain.Entities
{
    public class ClientEntity : IEntity
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
    }
}
