using System;
using CodeDesignPlus.Net.Microservice.gRpc.Core.FluentValidation;
using CodeDesignPlus.Net.Microservice.gRpc.Test.Helpers.Models;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

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
