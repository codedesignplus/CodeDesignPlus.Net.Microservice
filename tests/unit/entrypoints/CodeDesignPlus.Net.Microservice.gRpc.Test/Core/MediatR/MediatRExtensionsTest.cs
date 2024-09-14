using System;
using CodeDesignPlus.Net.Microservice.gRpc.Core.MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CodeDesignPlus.Net.Microservice.gRpc.Test.Core.MediatR;

public class MediatRExtensionsTest
{
    [Fact]
    public void AddMediatRR_ShouldRegisterMediatRServices()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddMediatRR();
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        var requestsHandler = services.Where(s => s.ServiceType.IsGenericType && s.ServiceType.GetGenericTypeDefinition() == typeof(IRequestHandler<>));
        var queriesHandler = services.Where(s => s.ServiceType.IsGenericType && s.ServiceType.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));
        var pipelineBehavior = services.FirstOrDefault(s => s.ServiceType.IsGenericType && s.ServiceType.GetGenericTypeDefinition() == typeof(IPipelineBehavior<,>));

        var mediatRService = serviceProvider.GetService<IMediator>();
        Assert.NotNull(mediatRService);
        Assert.NotEmpty(requestsHandler);
        Assert.NotEmpty(queriesHandler);
        Assert.NotNull(pipelineBehavior);
    }

}
