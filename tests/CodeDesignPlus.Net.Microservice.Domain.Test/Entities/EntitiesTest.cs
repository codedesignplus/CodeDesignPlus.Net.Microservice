

using System.Reflection;

namespace CodeDesignPlus.Net.Microservice.Domain.Test.Entities;

public class EntitiesTest
{

    [Fact]
    public void Entities_GetAndSetValues_Equals()
    {
        // Arrange
        var assembly = typeof(OrderAggregate).Assembly;

        var entities = assembly
            .GetTypes()
            .Where(x => typeof(IEntityBase).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract && !x.IsSubclassOf(typeof(AggregateRoot))); 

        // Act
        foreach (var entity in entities)
        {
            var instance = Activator.CreateInstance(entity);

            entity.SetValueProperties(instance);

            // Assert
            Assert.NotNull(instance);

            Assert.All(entity.GetProperties(), property =>
            {
                var value = property.GetValue(instance);

                var valueDefault = property.PropertyType.GetDefaultValue();

                Assert.NotEqual(valueDefault, value);
            });

        }
    }

}
