using System;
using System.Reflection;

namespace CodeDesignPlus.Net.Microservice.Application.Test.Helpers;


public static class Utils
{

    public static object? GetDefaultValue(this Type? type)
    {
        return type!.IsValueType ? Activator.CreateInstance(type) : null;
    }

    public static Dictionary<ParameterInfo, object> GetValues(this ParameterInfo[] properties)
    {
        var values = new Dictionary<ParameterInfo, object>();

        for (int i = 0; i < properties.Length; i++)
        {
            var property = properties[i];

            if (property.ParameterType == typeof(string))
            {
                values.Add(property, "Test");
            }
            else if (property.ParameterType == typeof(int))
            {
                values.Add(property, 1);
            }
            else if (property.ParameterType == typeof(long))
            {
                values.Add(property, 1L);
            }
            else if (property.ParameterType == typeof(Guid))
            {
                values.Add(property, Guid.NewGuid());
            }
            else if (property.ParameterType == typeof(DateTime))
            {
                values.Add(property, DateTime.UtcNow);
            }
            else if (property.ParameterType == typeof(DateTimeOffset))
            {
                values.Add(property, DateTimeOffset.UtcNow);
            }
            else if (property.ParameterType == typeof(bool))
            {
                values.Add(property, true);
            }
            else if (property.ParameterType == typeof(decimal))
            {
                values.Add(property, 1.0M);
            }
            else if (property.ParameterType == typeof(float))
            {
                values.Add(property, 1.0F);
            }
            else if (property.ParameterType == typeof(double))
            {
                values.Add(property, 1.0D);
            }
            else if (property.ParameterType == typeof(byte))
            {
                values.Add(property, (byte)1);
            }
            else if (property.ParameterType == typeof(short))
            {
                values.Add(property, (short)1);
            }
            else if (property.ParameterType == typeof(byte[]))
            {
                values.Add(property, new byte[] { 1, 2, 3 });
            }
            else if (property.ParameterType == typeof(int?))
            {
                values.Add(property, 1);
            }
            else if (property.ParameterType == typeof(long?))
            {
                values.Add(property, 1L);
            }
            else if (property.ParameterType == typeof(Guid?))
            {
                values.Add(property, Guid.NewGuid());
            }
            else if (property.ParameterType == typeof(DateTime?))
            {
                values.Add(property, DateTime.UtcNow);
            }
            else if (property.ParameterType == typeof(DateTimeOffset?))
            {
                values.Add(property, DateTimeOffset.UtcNow);
            }
            else if (property.ParameterType == typeof(bool?))
            {
                values.Add(property, true);
            }
            else if (property.ParameterType == typeof(decimal?))
            {
                values.Add(property, 1.0M);
            }
            else if (property.ParameterType == typeof(float?))
            {
                values.Add(property, 1.0F);
            }
            else if (property.ParameterType == typeof(double?))
            {
                values.Add(property, 1.0D);
            }
            else if (property.ParameterType == typeof(byte?))
            {
                values.Add(property, (byte)1);
            }
            else if (property.ParameterType == typeof(short?))
            {
                values.Add(property, (short)1);
            }
            else if (property.Name == "metadata")
            {
                values.Add(property, new Dictionary<string, object>());
            }
            else if (property.ParameterType.IsEnum)
            {
                values.Add(property, Enum.GetValues(property.ParameterType).GetValue(0)!);
            }
            else if (property.ParameterType.IsClass && !property.ParameterType.IsAbstract)
            {
                values.Add(property, Activator.CreateInstance(property.ParameterType)!);
            }

        }

        return values;
    }

    public static void SetValueProperties(this Type? type, object? instance)
    {
        var properties = type!.GetProperties();

        foreach (var property in properties)
        {
            if (property.PropertyType == typeof(string))
            {
                property.SetValue(instance, "Test");
            }
            else if (property.PropertyType == typeof(int))
            {
                property.SetValue(instance, 1);
            }
            else if (property.PropertyType == typeof(long))
            {
                property.SetValue(instance, 1L);
            }
            else if (property.PropertyType == typeof(Guid))
            {
                property.SetValue(instance, Guid.NewGuid());
            }
            else if (property.PropertyType == typeof(DateTime))
            {
                property.SetValue(instance, DateTime.UtcNow);
            }
            else if (property.PropertyType == typeof(DateTimeOffset))
            {
                property.SetValue(instance, DateTimeOffset.UtcNow);
            }
            else if (property.PropertyType == typeof(bool))
            {
                property.SetValue(instance, true);
            }
            else if (property.PropertyType == typeof(decimal))
            {
                property.SetValue(instance, 1.0M);
            }
            else if (property.PropertyType == typeof(float))
            {
                property.SetValue(instance, 1.0F);
            }
            else if (property.PropertyType == typeof(double))
            {
                property.SetValue(instance, 1.0D);
            }
            else if (property.PropertyType == typeof(byte))
            {
                property.SetValue(instance, (byte)1);
            }
            else if (property.PropertyType == typeof(short))
            {
                property.SetValue(instance, (short)1);
            }
            else if (property.PropertyType == typeof(byte[]))
            {
                property.SetValue(instance, new byte[] { 1, 2, 3 });
            }
            else if (property.PropertyType == typeof(int?))
            {
                property.SetValue(instance, 1);
            }
            else if (property.PropertyType == typeof(long?))
            {
                property.SetValue(instance, 1L);
            }
            else if (property.PropertyType == typeof(Guid?))
            {
                property.SetValue(instance, Guid.NewGuid());
            }
            else if (property.PropertyType == typeof(DateTime?))
            {
                property.SetValue(instance, DateTime.UtcNow);
            }
            else if (property.PropertyType == typeof(DateTimeOffset?))
            {
                property.SetValue(instance, DateTimeOffset.UtcNow);
            }
            else if (property.PropertyType == typeof(bool?))
            {
                property.SetValue(instance, true);
            }
            else if (property.PropertyType == typeof(decimal?))
            {
                property.SetValue(instance, 1.0M);
            }
            else if (property.PropertyType == typeof(float?))
            {
                property.SetValue(instance, 1.0F);
            }
            else if (property.PropertyType == typeof(double?))
            {
                property.SetValue(instance, 1.0D);
            }
            else if (property.PropertyType == typeof(byte?))
            {
                property.SetValue(instance, (byte)1);
            }
            else if (property.PropertyType == typeof(short?))
            {
                property.SetValue(instance, (short)1);
            }
            else if (property.PropertyType.IsEnum)
            {
                var values = Enum.GetValues(property.PropertyType);
                property.SetValue(instance, values.GetValue(values.Length - 1));
            }
            else if (property.PropertyType.IsClass && !property.PropertyType.IsAbstract)
            {
                property.SetValue(instance, Activator.CreateInstance(property.PropertyType));
            }

        }
    }
}
