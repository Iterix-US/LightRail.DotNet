# SerilogBuilder

Provides a fluent API for configuring and building a Serilog-based logger using the LightRail.DotNet framework.

**Note**: This class is designed to be used through the [LoggerFactoryBuilder](LoggerFactoryBuilder.md) class. Feel free to use this class, but the fluent syntax of the LoggerFactoryBuilder allows for easier switching between providers.

---

## 📦 Namespace

`LightRail.DotNet.Logging`

---

## ⚙️ Features

- Output logs to file and/or console
- Configure minimum logging level
- Configure rolling intervals and retention
- Customize output templates
- Inject logger into .NET DI container
- Load configuration from `IConfiguration` (e.g., `appsettings.json`)
- Supports both structured and traditional logging

---

## ✅ Usage

### Basic Configuration

```csharp
var logger = new SerilogBuilder()
    .WithMinimumLevel(LoggingLevel.Information)
    .WithRollingInterval(LogRollInterval.Daily)
    .WithFileSizeLimit(10 * 1024 * 1024)
    .WithFileRetentionLimit(7)
    .WithOutputTemplate("{Timestamp:HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}")
    .WithCategory("MyApp")
    .OutputToConsole()
    .OutputToFile(createLogPath: true, logPath: "C:\Logs", logName: "MyAppLog", logExtension: "txt")
    .Build();
```

### Load from IConfiguration

```csharp
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var logger = new SerilogBuilder()
    .FromConfiguration(configuration)
    .Build();
```

### Inject into Dependency Injection

```csharp
var services = new ServiceCollection();

new SerilogBuilder()
    .WithMinimumLevel(LoggingLevel.Debug)
    .OutputToConsole()
    .AddAsService(services, ServiceLifetime.Singleton);
```

---

## 🧪 Notes

- Once `Build()` is called, the logger is finalized and immutable.
- Calling `Build()` multiple times will throw unless you call `Reset()` first.
- The `LoggerLocation` will be printed to the log upon build if output is enabled.

---

## 🧼 Defaults

| Property              | Default Value                            |
|-----------------------|------------------------------------------|
| Log Path              | `AppContext.BaseDirectory`               |
| Log File Name         | `Log`                                    |
| Log File Extension    | `log`                                    |
| Rolling Interval      | `Monthly`                                |
| File Size Limit       | `10 MB`                                  |
| Retention Limit       | `31`                                     |
| Minimum Level         | `Debug`                                  |
| Output Template       | `{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}` |
| Category              | Assembly name or `"AppLogger"`           |
| Console Output        | Disabled                                 |
| File Output           | Disabled                                 |

---

## 📚 Related

- [LoggerFactoryBuilder](./LoggerFactoryBuilder.md)
- [NLogBuilder](./NLogBuilder.md)
