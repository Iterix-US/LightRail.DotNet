# LoggerFactoryBuilder

The `LoggerFactoryBuilder` class in `LightRail.DotNet.Logging` provides a unified, fluent API for constructing and configuring logger instances using either Serilog or NLog. This class simplifies logger creation by exposing configurable options through method chaining.

---

## 📦 Namespace

```csharp
LightRail.DotNet.Logging
```

## ⚙️ Summary

- Allows configuration of console and file output.
- Supports custom paths, rolling intervals, size limits, and output templates.
- Can generate loggers using either **Serilog** or **NLog**.
- Integrates with external `IConfiguration` sources.

---

## ✅ Usage

### Example: Creating a Serilog Logger

```csharp
var logger = new LoggerFactoryBuilder()
    .EnableConsoleOutput()
    .EnableFileOutput(createLogPath: true, logPath: "/logs", logName: "system", logExtension: "txt")
    .SetMinimumLevel(LoggingLevel.Information)
    .SetRollingInterval(LogRollInterval.Daily)
    .SetFileSizeLimit(10 * 1024 * 1024)
    .SetFileRetentionLimit(7)
    .SetOutputTemplate("{Timestamp} [{Level}] {Message}{NewLine}{Exception}")
    .SetCategory("System")
    .BuildSerilog();
```

### Example: Creating an NLog Logger

```csharp
var logger = new LoggerFactoryBuilder()
    .EnableConsoleOutput()
    .EnableFileOutput(createLogPath: true, logPath: "/logs", logName: "system", logExtension: "log")
    .SetMinimumLevel(LoggingLevel.Debug)
    .SetRollingInterval(LogRollInterval.Monthly)
    .SetFileSizeLimit(5 * 1024 * 1024)
    .SetFileRetentionLimit(30)
    .SetCategory("Application")
    .BuildNLog();
```

---

## 🔧 Fluent Methods

| Method | Description |
|--------|-------------|
| `EnableConsoleOutput()` | Enables logging to the console. |
| `EnableFileOutput(createLogPath, logPath, logName, logExtension)` | Enables file logging with custom options. |
| `SetMinimumLevel(LoggingLevel)` | Sets minimum logging level. |
| `SetRollingInterval(LogRollInterval)` | Configures time-based log file rolling. |
| `SetFileSizeLimit(long)` | Sets file size limit in bytes. |
| `SetFileRetentionLimit(int)` | Sets max number of retained log files. |
| `SetOutputTemplate(string)` | Defines the log message output template. |
| `SetCategory(string)` | Sets logger category (module name). |
| `SetConfiguration(IConfiguration)` | Injects external config (e.g., appsettings). |

---

## 🧪 Output Builders

- `BuildSerilog()` → `ILogger` via Serilog.
- `BuildNLog()` → `ILogger` via NLog.

Each method returns a configured `ILogger` instance.

---

## 📄 Requirements

- `Microsoft.Extensions.Logging`
- `Serilog` and `NLog` packages

---

## 🗂️ Related

- [`SerilogBuilder`](./SerilogBuilder.md)
- [`NLogBuilder`](./NLogBuilder.md)
- [`ILoggerBuilder`](./ILoggerBuilder.md)