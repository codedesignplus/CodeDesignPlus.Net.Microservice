using CodeDesignPlus.Net.Microservice.Application.Order.Commands.CancelOrder;

namespace CodeDesignPlus.Net.Microservice.Application.Test.Order.Commands.CancelOrder;

public class CancelOrderCommandTest
{
    private readonly Validator _validator;

    public CancelOrderCommandTest()
    {
        _validator = new Validator();
    }

    [Fact]
    public async Task Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new CancelOrderCommand(Guid.Empty, "Reason");
        var result = await _validator.ValidateAsync(command);

        Assert.Contains(result.Errors, x => x.PropertyName == nameof(command.Id) && !string.IsNullOrEmpty(x.ErrorMessage));
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task Should_Have_Error_When_Reason_Is_Invalid(string? reason)
    {
        var command = new CancelOrderCommand(Guid.NewGuid(), reason!);
        var result = await _validator.ValidateAsync(command);

        Assert.Contains(result.Errors, x => x.PropertyName == nameof(command.Reason) && !string.IsNullOrEmpty(x.ErrorMessage));
    }

    [Fact]
    public async Task Should_Not_Have_Error()
    {
        var command = new CancelOrderCommand(Guid.NewGuid(), "Reason");
        var result = await _validator.ValidateAsync(command);

        Assert.Empty(result.Errors);
    }

}
