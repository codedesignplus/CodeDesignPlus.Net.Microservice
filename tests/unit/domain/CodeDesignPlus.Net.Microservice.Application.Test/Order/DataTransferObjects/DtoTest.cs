using CodeDesignPlus.Net.Microservice.Application.Test.Helpers;
using System.Reflection;

namespace CodeDesignPlus.Net.Microservice.Application.Test.Order.DataTransferObjects;

public class DtoTest
{

    [Fact]
    public void Dtos_GetAndSet_Success()
    {
        // Arrange
        var dtos = typeof(ClientDto).Assembly.GetTypes()
            .Where(x => x.IsClass && x.IsPublic && x.Name.EndsWith("Dto"))
            .ToList();

        // Act
        foreach (var dto in dtos)
        {
            var instance = Activator.CreateInstance(dto);

            dto.SetValueProperties(instance);

            // Assert
            Assert.NotNull(instance);

            var properties = dto.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var value = property.GetValue(instance);

                Assert.NotNull(value);
                Assert.NotEqual(value, property.PropertyType.GetDefaultValue());
            }
        }
    }
}
