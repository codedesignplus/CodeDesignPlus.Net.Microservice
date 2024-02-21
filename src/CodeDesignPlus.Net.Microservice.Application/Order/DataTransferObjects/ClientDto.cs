namespace CodeDesignPlus.Net.Microservice.Application.Order.DataTransferObjects
{
    public class ClientDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public bool IsActive { get; set; }
        public Guid Tenant { get; set; }
        public Guid IdUserCreator { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
