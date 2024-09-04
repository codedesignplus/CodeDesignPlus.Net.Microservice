using CodeDesignPlus.Net.Microservice.Application.Order.Commands.RemoveProduct;

namespace CodeDesignPlus.Net.Microservice.Application.Test.Order.Commands.RemoveProduct;

public class RemoveProductCommandTest
{

    private readonly Validator _validator;

    public RemoveProductCommandTest()
    {
        _validator = new Validator();
    }

    [Fact]
    public async Task Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new RemoveProductCommand(Guid.Empty, Guid.NewGuid());
        var result = await _validator.ValidateAsync(command);

        Assert.Contains(result.Errors, x => x.PropertyName == nameof(command.Id) && !string.IsNullOrEmpty(x.ErrorMessage));
    }

    [Fact]
    public async Task Should_Have_Error_When_ProductId_Is_Empty()
    {
        var command = new RemoveProductCommand(Guid.NewGuid(), Guid.Empty);
        var result = await _validator.ValidateAsync(command);

        Assert.Contains(result.Errors, x => x.PropertyName == nameof(command.ProductId) && !string.IsNullOrEmpty(x.ErrorMessage));
    }

}
