namespace CodeDesignPlus.Net.Microservice.Domain.Entities;

public class ClientEntity : IEntityBase
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
}
