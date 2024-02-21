using CodeDesignPlus.Net.Microservice.Domain.ValueObjects;

namespace CodeDesignPlus.Net.Microservice.Application.Order.DataTransferObjects
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public DateTime? CompletionDate { get; set; }
        public DateTime? CancellationDate { get; set; }
        public ClientDto Client { get; set; } = default!;
        public List<ProductDto> Products { get; set; } = [];
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ReasonForCancellation { get; set; }
    }
}
