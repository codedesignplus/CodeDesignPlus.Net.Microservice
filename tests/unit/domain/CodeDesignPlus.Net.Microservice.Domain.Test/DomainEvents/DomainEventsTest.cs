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


    [Fact]
    public void NameConstructor_CreateInstance_CustomValues()
    {
        var domainEvents = typeof(OrderCancelledDomainEvent).Assembly
            .GetTypes()
            .Where(x => typeof(IDomainEvent).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            var nameConstructor = domainEvent.GetMethod("Create", BindingFlags.Static | BindingFlags.Public);

            Assert.NotNull(nameConstructor);

            var parameters = nameConstructor.GetParameters();

            Assert.NotEmpty(parameters);

            var values = parameters.GetValues();

            var instance = nameConstructor.Invoke(null, [.. values.Values]);

            Assert.NotNull(instance);

            var property = domainEvent.GetProperty(nameof(DomainEvent.AggregateId));

            var value = property!.GetValue(instance, null);

            var valueExpected = property.PropertyType.GetDefaultValue();

            Assert.NotEqual(valueExpected, value);
        }
    }
}
