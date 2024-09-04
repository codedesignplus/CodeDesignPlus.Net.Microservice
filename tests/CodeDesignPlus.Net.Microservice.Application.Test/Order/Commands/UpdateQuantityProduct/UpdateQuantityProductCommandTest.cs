using CodeDesignPlus.Net.Microservice.Application.Order.Commands.UpdateQuantityProduct;

namespace CodeDesignPlus.Net.Microservice.Application.Test.Order.Commands.UpdateQuantityProduct;

public class UpdateQuantityProductCommandTest
{

    private readonly Validator _validator;

    public UpdateQuantityProductCommandTest()
    {
        _validator = new Validator();
    }

    [Fact]
    public async Task Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new UpdateQuantityProductCommand(Guid.Empty, Guid.NewGuid(), 1);
        var result = await _validator.ValidateAsync(command);

        Assert.Contains(result.Errors, x => x.PropertyName == nameof(command.Id) && !string.IsNullOrEmpty(x.ErrorMessage));
    }

    [Fact]
    public async Task Should_Have_Error_When_ProductId_Is_Empty()
    {
        var command = new UpdateQuantityProductCommand(Guid.NewGuid(), Guid.Empty, 1);
        var result = await _validator.ValidateAsync(command);

        Assert.Contains(result.Errors, x => x.PropertyName == nameof(command.ProductId) && !string.IsNullOrEmpty(x.ErrorMessage));
    }

    [Fact]
    public async Task Should_Have_Error_When_Quantity_Is_Less_Than_One()
    {
        var command = new UpdateQuantityProductCommand(Guid.NewGuid(), Guid.NewGuid(), 0);
        var result = await _validator.ValidateAsync(command);

        Assert.Contains(result.Errors, x => x.PropertyName == nameof(command.Quantity) && !string.IsNullOrEmpty(x.ErrorMessage));
    }

    [Fact]
    public async Task Should_Not_Have_Error()
    {
        var command = new UpdateQuantityProductCommand(Guid.NewGuid(), Guid.NewGuid(), 1);
        var result = await _validator.ValidateAsync(command);

        Assert.Empty(result.Errors);
    }

}
