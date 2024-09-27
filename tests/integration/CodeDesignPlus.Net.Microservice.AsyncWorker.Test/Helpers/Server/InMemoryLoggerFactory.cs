using System;
using Microsoft.Extensions.Logging;

namespace CodeDesignPlus.Net.Microservice.AsyncWorker.Test.Helpers.Server;

public class InMemoryLoggerFactory : ILoggerFactory
{
    private readonly ILoggerProvider _loggerProvider;

    public InMemoryLoggerFactory(InMemoryLoggerProvider provider)
    {
        _loggerProvider = provider;
    }

    public void AddProvider(ILoggerProvider provider)
    {
        // Aquí puedes manejar los proveedores de log adicionales si es necesario
    }

    public ILogger CreateLogger(string categoryName)
    {
        return _loggerProvider.CreateLogger(categoryName);
    }

    public void Dispose()
    {
        _loggerProvider?.Dispose();
    }
}
