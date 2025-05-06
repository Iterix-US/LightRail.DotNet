# NLogBuilder

The `NLogBuilder` class provides a configurable way to create and manage NLog-based logging in .NET applications, following a fluent builder pattern.

**Note**: This class is designed to be used through the [LoggerFactoryBuilder](LoggerFactoryBuilder.md) class. Feel free to use this class, but the fluent syntax of the LoggerFactoryBuilder allows for easier switching between providers.

---

## 📦 Namespace

`LightRail.DotNet.Logging`

---

## 🧰 Purpose

Wraps the setup of NLog logging using a clean, extensible builder-style interface. It allows for structured configuration of file and console output, rolling intervals, file limits, and more.

---

## ✅ Usage Example

```csharp
var logger = new NLogBuilder()
    .WithMinimumLevel(LoggingLevel.Information)
    .OutputToConsole()
    .OutputToFile(createLogPath: true, logPath: "logs", logName: "app", logExtension: "log")
    .WithRollingInterval(LogRollInterval.Daily)
    .WithFileSizeLimit(1024 * 1024 * 10) // 10 MB
    .WithFileRetentionLimit(7)
    .Build();
```

---

## 🔧 Key Methods

| Method | Description |
|--------|-------------|
| `OutputToConsole()` | Enables console logging. |
| `OutputToFile()` | Enables logging to a file, with options for path, name, and extension. |
| `WithMinimumLevel()` | Sets the minimum severity level for logs. |
| `WithRollingInterval()` | Configures how often logs roll over (e.g., Daily, Monthly). |
| `WithFileSizeLimit()` | Sets the max size per log file before rolling. |
| `WithFileRetentionLimit()` | Sets how many old log files to retain. |
| `WithOutputTemplate()` | Customizes the log formatting string. |
| `WithEnhancement()` | Applies a user-defined transformation to the `LoggingConfiguration`. |
| `AddAsService()` | Registers the logger with a DI container. |
| `FromConfiguration()` | Loads logger configuration from `IConfiguration`. |
| `Build()` | Builds and returns the `ILogger` instance. |

---

## 💡 Notes

- Default log file location: `AppContext.BaseDirectory`
- Configuration fallback: If `WithConfiguration()` is not called, the builder builds its own `LoggingConfiguration`.
- Supports use in ASP.NET Core by calling `AddAsService()` with the DI container.

---

## 🧪 Tested via

`LoggerFactoryBuilder` integration tests