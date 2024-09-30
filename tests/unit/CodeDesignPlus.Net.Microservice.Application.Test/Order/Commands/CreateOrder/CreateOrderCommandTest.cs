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
        var command = new CreateOrderCommand(Guid.NewGuid(), null!);
        var result = await _validator.ValidateAsync(command);

        Assert.Contains(result.Errors, x => x.PropertyName == nameof(command.Client) && !string.IsNullOrEmpty(x.ErrorMessage));
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task Should_Have_Error_When_ClientName_Is_Invalid(string? client)
    {
        var command = new CreateOrderCommand(Guid.NewGuid(), new ClientDto()
        {
            Name = client!
        });
        var result = await _validator.ValidateAsync(command);

        Assert.Contains(result.Errors, x => x.PropertyName == $"{nameof(CreateOrderCommand.Client)}.{nameof(CreateOrderCommand.Client.Id)}" && !string.IsNullOrEmpty(x.ErrorMessage));
    }

    [Fact]
    public async Task Shoul_Have_Error_When_ClientId_Is_Empty()
    {
        var command = new CreateOrderCommand(Guid.NewGuid(), new ClientDto()
        {
            Id = Guid.Empty,
            Name = "Client"
        });
        var result = await _validator.ValidateAsync(command);

        Assert.Contains(result.Errors, x => x.PropertyName == $"{nameof(command.Client)}.{nameof(command.Client.Id)}" && !string.IsNullOrEmpty(x.ErrorMessage));
    }

    [Fact]
    public async Task Should_Not_Have_Error()
    {
        var command = new CreateOrderCommand(Guid.NewGuid(), new ClientDto()
        {
            Id = Guid.NewGuid(),
            Name = "Client"
        });
        var result = await _validator.ValidateAsync(command);

        Assert.Empty(result.Errors);
    }

}
