using Microsoft.Extensions.Logging;

namespace RegressionTestHarness.Utilities;

internal class ListBoxLoggerProvider(Action<string> logAction) : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName) => new ListBoxLogger(categoryName, logAction);
    public void Dispose() { }
}