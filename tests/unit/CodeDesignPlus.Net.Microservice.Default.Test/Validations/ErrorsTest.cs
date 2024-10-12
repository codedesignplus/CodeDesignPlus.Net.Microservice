namespace CodeDesignPlus.Net.Microservice.Default.Test.Validations;

/// <summary>
/// A class for validating error formats.
/// </summary>
public class ErrorTests
{
    /// <summary>
    /// Validates that error messages follow the correct format.
    /// </summary>
    [Theory]
    [Errors<Application.Errors>]
    public void Errors_CheckFormat_Infrastructure(FieldInfo error, object value)
    {
        // Assert
        Assert.NotNull(error);
        Assert.NotNull(value);
        Assert.NotEmpty(value.ToString()!);

        var pattern = @"^\d{3} : .+$";
        Assert.Matches(pattern, value.ToString());
    }

    /// <summary>
    /// Validates that error messages follow the correct format.
    /// </summary>
    [Theory]
    [Errors<Infrastructure.Errors>]
    public void Errors_CheckFormat_Application(FieldInfo error, object value)
    {
        // Assert
        Assert.NotNull(error);
        Assert.NotNull(value);
        Assert.NotEmpty(value.ToString()!);

        var pattern = @"^\d{3} : .+$";
        Assert.Matches(pattern, value.ToString());
    }
    
    /// <summary>
    /// Validates that error messages follow the correct format.
    /// </summary>
    [Theory]
    [Errors<Domain.Errors>]
    public void Errors_CheckFormat_Domain(FieldInfo error, object value)
    {
        // Assert
        Assert.NotNull(error);
        Assert.NotNull(value);
        Assert.NotEmpty(value.ToString()!);

        var pattern = @"^\d{3} : .+$";
        Assert.Matches(pattern, value.ToString());
    }
}