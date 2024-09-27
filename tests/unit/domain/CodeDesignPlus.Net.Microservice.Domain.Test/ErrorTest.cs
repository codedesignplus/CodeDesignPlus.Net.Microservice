using System.Reflection;

namespace CodeDesignPlus.Net.Microservice.Domain.Test;

public class ErrorTest
{

    [Fact]
    public void Errors_CheckFormat_Success()
    {
        // Arrange
        var errors = typeof(Errors).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(x => x.FieldType == typeof(string))
            .ToList();

        // Act
        foreach (var error in errors)
        {
            var value = error.GetValue(null);

            // Assert
            Assert.NotNull(value);
            Assert.NotEmpty(value.ToString()!);

            string pattern = @"^\d{3} : .+$";
            Assert.Matches(pattern, value.ToString());
        }
    }
}
