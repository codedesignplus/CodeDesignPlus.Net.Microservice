namespace CodeDesignPlus.Net.Microservice.Application.Order.DataTransferObjects;

public class ClientDto : IDtoBase
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Document { get; set; }
    public string? TypeDocument { get; set; }
}
