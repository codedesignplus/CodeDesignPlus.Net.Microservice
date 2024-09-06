namespace CodeDesignPlus.Net.Microservice.Rest.Core.MediatR;

public static class MediatRExtensions
{
    public static IServiceCollection AddMediatRR(this IServiceCollection services)
    {
        var assemblyApplication = AppDomain.CurrentDomain.Load("CodeDesignPlus.Net.Microservice.Application");

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblyApplication));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipeline<,>));

        return services;
    }
}
