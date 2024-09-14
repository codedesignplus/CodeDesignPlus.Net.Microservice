using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeDesignPlus.Net.Microservice.AsyncWorker.Test;

public class StartupServicesTest
{
    [Fact]
    public void Initialize_ShouldConfigureServices_Success()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build();
        var startup = new StartupServices();

        // Act
        var exception = Record.Exception(() => startup.Initialize(services, configuration));

        // Assert
        Assert.Null(exception);
    }
}
