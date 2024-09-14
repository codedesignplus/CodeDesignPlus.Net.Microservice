using System;
using System.Reflection;
using CodeDesignPlus.Net.Microservice.Application.Order.Queries.FindOrderById;
using CodeDesignPlus.Net.Microservice.Application.Test.Helpers;
using MediatR;
using System.Linq;

namespace CodeDesignPlus.Net.Microservice.Application.Test.Order.Queries;

public class QueriesTest
{

    [Fact]
    public void Queries_GetAndSet_Success()
    {
        // Arrange
         var queries = typeof(FindOrderByIdQuery).Assembly.GetTypes()
            .Where(x => x.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequest<>)))
            .ToList();

        // Act
        foreach (var query in queries)
        {
            var constructor = query.GetConstructors().FirstOrDefault()!;

            var parameters = constructor.GetParameters();

            var values = parameters.GetValues();

            var instance = constructor.Invoke([.. values.Values]);

            // Assert

            Assert.NotNull(instance);

            var properties = query.GetProperties(BindingFlags.Public | BindingFlags.Instance);

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
