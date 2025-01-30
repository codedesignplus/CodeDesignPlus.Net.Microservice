namespace CodeDesignPlus.Net.Microservice.Default.Test.Validations;

/// <summary>
/// A class for validating startup services.
/// </summary>
public class StartupTest
{
    /// <summary>
    /// Validates that the startup services do not throw exceptions during initialization.
    /// </summary>
    [Theory]
    [Startup<Domain.Startup>]
    public void Sturtup_CheckNotThrowException_Domain(IStartup startup, Exception exception)
    {
        // Assert
        Assert.NotNull(startup);
        Assert.Null(exception);
    }

    /// <summary>
    /// Validates that the startup services do not throw exceptions during initialization.
    /// </summary>
    [Theory]
    [Startup<Application.Startup>]
    public void Sturtup_CheckNotThrowException_Application(IStartup startup, Exception exception)
    {
        // Assert
        Assert.NotNull(startup);
        Assert.Null(exception);
    }

    /// <summary>
    /// Validates that the startup services do not throw exceptions during initialization.
    /// </summary>
    [Theory]
    [Startup<Infrastructure.Startup>]
    public void Sturtup_CheckNotThrowException_Infrastructure(IStartup startup, Exception exception)
    {
        // Assert
        Assert.NotNull(startup);
        Assert.Null(exception);
    }
}