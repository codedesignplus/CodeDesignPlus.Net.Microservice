using System.Reflection;

namespace CodeDesignPlus.Net.Microservice.Domain.Test;

public class UnitTest1
{
    [Fact]
    public void Constructor_Instance()
    {
        var aggregates = typeof(OrderAggregate).Assembly
            .GetTypes()
            .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(typeof(AggregateRoot)))
            .ToList();

        foreach (var aggregate in aggregates)
        {
            var constructor = aggregate.GetConstructor([typeof(Guid)]);

            Assert.NotNull(constructor);

            var valueExpected = Guid.NewGuid();

            var instance = constructor.Invoke([valueExpected]);

            var value = aggregate.GetProperty(nameof(AggregateRoot.Id))!.GetValue(instance, null);

            Assert.NotNull(instance);
            Assert.Equal(valueExpected, value);
        }
    }

    [Fact]
    public void NameConstructor_CreateInstance_CustomValues()
    {
        var aggregates = typeof(OrderAggregate).Assembly
            .GetTypes()
            .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(typeof(AggregateRoot)))
            .ToList();

        foreach (var aggregate in aggregates)
        {
            var nameConstructor = aggregate.GetMethod("Create", BindingFlags.Static | BindingFlags.Public);

            Assert.NotNull(nameConstructor);

            var parameters = nameConstructor.GetParameters();

            Assert.NotEmpty(parameters);

            var values = parameters.GetValues();

            var instance = nameConstructor.Invoke(null, [.. values.Values]);

            Assert.NotNull(instance);

            var value = aggregate.GetProperty(nameof(AggregateRoot.Id))!.GetValue(instance, null);
            var valueExpected = values.First(x => x.Key.Name!.Equals(nameof(AggregateRoot.Id), StringComparison.OrdinalIgnoreCase)).Value;

            Assert.NotNull(instance);
            Assert.Equal(valueExpected, value);
        }
    }
}