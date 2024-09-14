namespace CodeDesignPlus.Net.Microservice.gRpc.Core.FluentValidation;

public static class FluentExtensions
{
    public static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        var validator = AppDomain.CurrentDomain
                   .GetAssemblies()
                   .SelectMany(x => x.GetTypes())
                   .FirstOrDefault(type => type.BaseType?.IsGenericType == true && type.BaseType.GetGenericTypeDefinition() == typeof(AbstractValidator<>));

        ArgumentNullException.ThrowIfNull(validator);

        services.AddValidatorsFromAssembly(validator.Assembly);

        return services;
    }
}
