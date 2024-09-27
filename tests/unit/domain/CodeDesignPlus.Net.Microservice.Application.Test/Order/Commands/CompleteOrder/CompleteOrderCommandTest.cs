using CodeDesignPlus.Net.Microservice.Application.Order.Commands.CompleteOrder;

namespace CodeDesignPlus.Net.Microservice.Application.Test.Order.Commands.CompleteOrder;

public class CompleteOrderCommandTest
{

    private readonly Validator _validator;

    public CompleteOrderCommandTest()
    {
        _validator = new Validator();
    }

    [Fact]
    public async Task Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new CompleteOrderCommand(Guid.Empty);
        var result = await _validator.ValidateAsync(command);

        Assert.Contains(result.Errors, x => x.PropertyName == nameof(command.Id) && !string.IsNullOrEmpty(x.ErrorMessage));
    }

    [Fact]
    public async Task Should_Not_Have_Error()
    {
        var command = new CompleteOrderCommand(Guid.NewGuid());
        var result = await _validator.ValidateAsync(command);

        Assert.Empty(result.Errors);
    }
}
