using Microsoft.Extensions.Logging;

namespace RegressionTestHarness.Utilities;

public class CompositeLogger(params ILogger[] loggers) : ILogger
{
    public IDisposable BeginScope<TState>(TState state) => NullScope.Instance;
    public bool IsEnabled(LogLevel logLevel) => loggers.Any(l => l.IsEnabled(logLevel));

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        foreach (var logger in loggers)
        {
            logger.Log(logLevel, eventId, state, exception, formatter);
        }
    }

    private class NullScope : IDisposable
    {
        public static readonly NullScope Instance = new();
        public void Dispose() { }
    }
}