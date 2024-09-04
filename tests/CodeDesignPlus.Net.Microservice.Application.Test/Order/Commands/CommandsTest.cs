using System;
using System.Reflection;
using CodeDesignPlus.Net.Generator;
using CodeDesignPlus.Net.Microservice.Application.Order.Commands.CreateOrder;
using CodeDesignPlus.Net.Microservice.Application.Test.Helpers;
using MediatR;

namespace CodeDesignPlus.Net.Microservice.Application.Test.Order.Commands;

public class CommandsTest
{

    [Fact]
    public void Commands_GetAndSet_Success()
    {
        // Arrange
        var commands = typeof(CreateOrderCommand).Assembly.GetTypes()
            .Where(x => typeof(IRequest).IsAssignableFrom(x))
            .ToList();

        // Act
        foreach (var command in commands)
        {
            var constructor = command.GetConstructors().FirstOrDefault()!;

            var parameters = constructor.GetParameters();

            var values = parameters.GetValues();

            var instance = Activator.CreateInstance(command, [.. values.Values]);

            // Assert
            Assert.NotNull(instance);

            var properties = command.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var value = property.GetValue(instance);
                var valueExpected = values.FirstOrDefault(x => x.Key.Name == property.Name).Value;

                Assert.NotNull(value);
                Assert.Equal(valueExpected, value);
            }
        }
    }
}
