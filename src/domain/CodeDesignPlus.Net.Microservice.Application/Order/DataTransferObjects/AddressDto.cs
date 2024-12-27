namespace CodeDesignPlus.Net.Microservice.Application.Order.DataTransferObjects;

public class AddressDto
{
    public required string Country { get; set; }
    public required string State { get; set; }
    public required string City { get; set; }
    public required string Address { get; set; }
    public int CodePostal { get; set; }

}
