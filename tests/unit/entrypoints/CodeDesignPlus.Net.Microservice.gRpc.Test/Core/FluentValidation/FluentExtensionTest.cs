namespace CodeDesignPlus.Net.Microservice.gRpc.Test.Core.FluentValidation;

public class FluentExtensionsTest
{
    [Fact]
    public void AddFluentValidation_ShouldRegisterValidators()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddFluentValidation();
        var serviceProvider = services.BuildServiceProvider();
        var validators = serviceProvider.GetServices<IValidator<UserModel>>();

        // Assert
        Assert.NotNull(validators);
        Assert.NotEmpty(validators);
        Assert.NotEmpty(services);
    }
}
