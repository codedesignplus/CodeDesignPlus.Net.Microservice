using CodeDesignPlus.Net.Microservice.Application.Order.Commands.CreateOrder;

namespace CodeDesignPlus.Net.Microservice.Application.Test.Order.Commands.CreateOrder;

public class CreateOrderCommandTest
{
    private readonly Validator _validator;

    public CreateOrderCommandTest()
    {
        _validator = new Validator();
    }

    [Fact]
    public async Task Should_Have_Error_When_Client_Is_Null()
    {
        var address = new AddressDto()
        {
            Country = "Colombia",
            State = "Bogota",
            City = "Bogota",
            Address = "Calle 123",
            CodePostal = 123456
        };
        var command = new CreateOrderCommand(Guid.NewGuid(), null!, address);
        var result = await _validator.ValidateAsync(command);

        Assert.Contains(result.Errors, x => x.PropertyName == nameof(command.Client) && !string.IsNullOrEmpty(x.ErrorMessage));
    }

    [Fact]
    public async Task Should_Have_Error_When_Address_Is_Null()
    {
        var client = new ClientDto()
        {
            Id = Guid.NewGuid(),
            Name = "Client",
            Document = "1234567890",
            TypeDocument = "CC"
        };
        var command = new CreateOrderCommand(Guid.NewGuid(), client, null!);
        var result = await _validator.ValidateAsync(command);

        Assert.Contains(result.Errors, x => x.PropertyName == nameof(command.Address) && !string.IsNullOrEmpty(x.ErrorMessage));
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task Should_Have_Error_When_ClientName_Is_Invalid(string? client)
    {
        var address = new AddressDto()
        {
            Country = "Colombia",
            State = "Bogota",
            City = "Bogota",
            Address = "Calle 123",
            CodePostal = 123456
        };

        var command = new CreateOrderCommand(Guid.NewGuid(), new ClientDto()
        {
            Name = client!,
            Document = "1234567890",
            TypeDocument = "CC"
        }, address);

        var result = await _validator.ValidateAsync(command);

        Assert.Contains(result.Errors, x => x.PropertyName == $"{nameof(CreateOrderCommand.Client)}.{nameof(CreateOrderCommand.Client.Id)}" && !string.IsNullOrEmpty(x.ErrorMessage));
    }

    [Fact]
    public async Task Shoul_Have_Error_When_ClientId_Is_Empty()
    {
        var address = new AddressDto()
        {
            Country = "Colombia",
            State = "Bogota",
            City = "Bogota",
            Address = "Calle 123",
            CodePostal = 123456
        };

        var command = new CreateOrderCommand(Guid.NewGuid(), new ClientDto()
        {
            Id = Guid.Empty,
            Name = "Client",
            Document = "1234567890",
            TypeDocument = "CC"
        }, address);

        var result = await _validator.ValidateAsync(command);

        Assert.Contains(result.Errors, x => x.PropertyName == $"{nameof(command.Client)}.{nameof(command.Client.Id)}" && !string.IsNullOrEmpty(x.ErrorMessage));
    }

    [Fact]
    public async Task Should_Not_Have_Error()
    {
        var address = new AddressDto()
        {
            Country = "Colombia",
            State = "Bogota",
            City = "Bogota",
            Address = "Calle 123",
            CodePostal = 123456
        };

        var command = new CreateOrderCommand(Guid.NewGuid(), new ClientDto()
        {
            Id = Guid.NewGuid(),
            Name = "Client",
            Document = "1234567890",
            TypeDocument = "CC"
        }, address);
        var result = await _validator.ValidateAsync(command);

        Assert.Empty(result.Errors);
    }

}
