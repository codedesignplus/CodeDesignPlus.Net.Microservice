
using CodeDesignPlus.Net.Core.Abstractions;
using CodeDesignPlus.Net.Exceptions.Extensions;
using CodeDesignPlus.Net.Microservice.Application.Order.Commands.AddProductToOrder;
using CodeDesignPlus.Net.Microservice.Domain;
using CodeDesignPlus.Net.Microservice.Domain.Repositories;
using CodeDesignPlus.Net.PubSub.Abstractions;
using FluentValidation.TestHelper;
using Moq;

namespace CodeDesignPlus.Net.Microservice.Application.Test.Order.Commands.AddProductToOrder;

public class AddProductToOrderCommandTest
{
    private readonly Validator _validator;

    public AddProductToOrderCommandTest()
    {
        _validator = new Validator();
    }

    [Fact]
    public async Task Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new AddProductToOrderCommand(Guid.Empty, Guid.NewGuid(), "product", "description", 1, 10);
        var result = await _validator.ValidateAsync(command);

        Assert.Contains(result.Errors, x => x.PropertyName == nameof(command.Id) && !string.IsNullOrEmpty(x.ErrorMessage));
    }

    [Fact]
    public async Task Should_Have_Error_When_IdProduct_Is_Empty()
    {
        var command = new AddProductToOrderCommand(Guid.NewGuid(), Guid.Empty, "product", "description", 1, 10);
        var result = await _validator.ValidateAsync(command);

        Assert.Contains(result.Errors, x => x.PropertyName == nameof(command.IdProduct) && !string.IsNullOrEmpty(x.ErrorMessage));
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task Should_Have_Error_When_Name_Is_Invalid(string name)
    {
        var command = new AddProductToOrderCommand(Guid.NewGuid(), Guid.NewGuid(), name, "description", 1, 10);
        var result = await _validator.ValidateAsync(command);

        Assert.Contains(result.Errors, x => x.PropertyName == nameof(command.Name) && !string.IsNullOrEmpty(x.ErrorMessage));
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task Should_Have_Error_When_Description_Is_Invalid(string description)
    {
        var command = new AddProductToOrderCommand(Guid.NewGuid(), Guid.NewGuid(), "product", description, 1, 10);
        var result = await _validator.ValidateAsync(command);

        Assert.Contains(result.Errors, x => x.PropertyName == nameof(command.Description) && !string.IsNullOrEmpty(x.ErrorMessage));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public async Task Should_Have_Error_When_Price_Is_Invalid(long price)
    {
        var command = new AddProductToOrderCommand(Guid.NewGuid(), Guid.NewGuid(), "product", "description", price, 10);
        var result = await _validator.ValidateAsync(command);

        Assert.Contains(result.Errors, x => x.PropertyName == nameof(command.Price) && !string.IsNullOrEmpty(x.ErrorMessage));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public async Task Should_Have_Error_When_Quantity_Is_Invalid(int quantity)
    {
        var command = new AddProductToOrderCommand(Guid.NewGuid(), Guid.NewGuid(), "product", "description", 1, quantity);
        var result = await _validator.ValidateAsync(command);

        Assert.Contains(result.Errors, x => x.PropertyName == nameof(command.Quantity) && !string.IsNullOrEmpty(x.ErrorMessage));
    }

    [Fact]
    public async Task Should_Not_Have_Error_When_All_Properties_Are_Valid()
    {
        var command = new AddProductToOrderCommand(Guid.NewGuid(), Guid.NewGuid(), "product", "description", 1, 10);
        var result = await _validator.ValidateAsync(command);

        Assert.Empty(result.Errors);
    }
}
