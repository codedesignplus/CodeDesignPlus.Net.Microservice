using System;
using System.Reflection;

namespace CodeDesignPlus.Net.Microservice.Domain.Test.DomainEvents;

public class DomainEvents
{

    [Fact]
    public void DomainEvents_GetAndSet_CheckValues()
    {
        // Arrange
        var assembly = typeof(OrderCancelledDomainEvent).Assembly;

        var domainEvents = assembly
            .GetTypes()
            .Where(x => typeof(IDomainEvent).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract);

        // Act
        foreach (var domainEvent in domainEvents)
        {

            var constructor = domainEvent.GetConstructors().FirstOrDefault()!;

            // Get the parameters of the selected constructor
            var parameters = constructor.GetParameters();

            var data = parameters.GetValues();

            var values = data.Values.ToArray();

            // Create an instance using the constructor (assuming it has no parameters for simplicity)
            var instance = constructor.Invoke(values);

            // Assert
            Assert.NotNull(instance);

            Assert.All(domainEvent.GetProperties(), property =>
            {
                var value = property.GetValue(instance);

                var valueExpected = data.FirstOrDefault(x => x.Key.Name!.Equals(property.Name, StringComparison.OrdinalIgnoreCase)).Value;

                Assert.Equal(valueExpected, value);
            });

        }
    }
}
