using System;
using Microsoft.Extensions.Logging;

namespace CodeDesignPlus.Net.Microservice.AsyncWorker.Test.Helpers.Server;

public class InMemoryLogger : ILogger
{
    private readonly string _name;
    private readonly List<string> _logs = new();

    public InMemoryLogger(string name)
    {
        _name = name;
    }

    public IDisposable BeginScope<TState>(TState state) => null;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        _logs.Add(formatter(state, exception));
    }

    public List<string> Logs => _logs;
}