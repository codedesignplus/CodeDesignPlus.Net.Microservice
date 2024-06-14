using FluentValidation;

namespace CodeDesignPlus.Net.Microservice.Core.FluentValidation
{
    public static class FluentExtensions
    {
        public static IServiceCollection AddFluentValidation(this IServiceCollection services)
        {
            // Add Fluent Validation
            var validator = AppDomain.CurrentDomain
                       .GetAssemblies()
                       .SelectMany(x => x.GetTypes())
                       .Where(type => type.BaseType?.IsGenericType == true && type.BaseType.GetGenericTypeDefinition() == typeof(AbstractValidator<>))
                       .FirstOrDefault();

            ArgumentNullException.ThrowIfNull(validator, nameof(validator));

            services.AddValidatorsFromAssembly(validator.Assembly);

            return services;
        }
    }
}
