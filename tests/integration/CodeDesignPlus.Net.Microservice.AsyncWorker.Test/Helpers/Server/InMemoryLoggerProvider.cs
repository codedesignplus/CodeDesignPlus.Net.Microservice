using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace CodeDesignPlus.Net.Microservice.AsyncWorker.Test.Helpers.Server;

public class InMemoryLoggerProvider : ILoggerProvider
{
    private readonly ConcurrentDictionary<string, InMemoryLogger> _loggers = new();

    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, name => new InMemoryLogger(name));
    }

    public void Dispose()
    {
        _loggers.Clear();
    }

    public ConcurrentDictionary<string, InMemoryLogger> Loggers => _loggers;
}