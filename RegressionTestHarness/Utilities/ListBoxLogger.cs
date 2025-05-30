using Microsoft.Extensions.Logging;
using SeroGlint.DotNet.Logging;
using SeroGlint.DotNet;
using System.Windows.Controls;
using System.Windows.Threading;

namespace RegressionTestHarness.Utilities;

public class ListBoxLogger(string category, Action<string> logAction) : ILogger
{
    public IDisposable BeginScope<TState>(TState state) => null!;
    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId,
        TState state, Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        var message = $"{DateTime.Now:HH:mm:ss} [{logLevel}] {category}: {formatter(state, exception)}";

        if (exception != null)
        {
            message += $" | {exception.GetType().Name}: {exception.Message}";
        }

        logAction(message);
    }

    internal static ILogger RouteLoggerToListBox(Dispatcher dispatcher, ListBox targetListBox, string category)
    {
        var coreLogger = new LoggerFactoryBuilder()
            .EnableConsoleOutput()
            .SetMinimumLevel(LoggingLevel.Debug)
            .SetOutputTemplate("{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .SetCategory(category)
            .BuildSerilog();

        var factory = LoggerFactory.Create(builder =>
        {
            builder.AddProvider(new ListBoxLoggerProvider(message => AppendLog(dispatcher, targetListBox, message)));
        });

        return new CompositeLogger(coreLogger, factory.CreateLogger(category));
    }

    private static void AppendLog(Dispatcher dispatcher, ListBox listBoxControl, string message)
    {
        dispatcher.Invoke(() =>
        {
            listBoxControl.Items.Add(message);
            listBoxControl.ScrollIntoView(message);
        });
    }
}