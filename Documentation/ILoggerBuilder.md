# ILoggerBuilder&lt;TLoggerBuilder, TLogger, TLoggerConfiguration&gt;

Defines a generic contract for building and configuring logging frameworks in a consistent, fluent, and strongly-typed manner.

---

## 📦 Namespace

`SeroGlint.DotNet.Logging.LoggerInterfaces`

---

## 🧰 Type Parameters

- `TLoggerBuilder`: The concrete type of the logger builder.
- `TLogger`: The concrete type of the logger (e.g., Serilog, NLog, etc.).
- `TLoggerConfiguration`: The configuration object used by the logger.

---

## ✅ Methods & Properties

### Properties

- `TLogger Instance`: Gets the built logger instance.
- `TLoggerConfiguration Configuration`: Gets the current configuration.
- `bool IsBuilt`: Indicates whether the logger has already been built.

### Fluent Configuration Methods

- `TLoggerBuilder OutputToFile(bool createLogPath = false, string logPath = null, string logName = null, string logExtension = null)`  
  Enable log file output.

- `TLoggerBuilder OutputToConsole()`  
  Enable console logging.

- `TLoggerBuilder WithMinimumLevel(LoggingLevel logLevel)`  
  Set minimum log level.

- `TLoggerBuilder WithRollingInterval(LogRollInterval rollingInterval)`  
  Set log file rotation strategy.

- `TLoggerBuilder WithFileSizeLimit(long fileSizeLimit)`  
  Set max file size before rolling.

- `TLoggerBuilder WithFileRetentionLimit(int fileCount)`  
  Set number of retained log files.

- `TLoggerBuilder WithOutputTemplate(string outputTemplate)`  
  Set the log formatting template.

- `TLoggerBuilder WithConfiguration(TLoggerConfiguration configuration)`  
  Inject a complete configuration.

- `TLoggerBuilder WithEnhancement(Func&lt;TLoggerConfiguration, TLoggerConfiguration&gt; enhancement)`  
  Apply custom enhancements via delegate.

- `TLoggerBuilder WithCategory(string category)`  
  Set logger category/module.

- `TLoggerBuilder FromConfiguration(IConfiguration configuration)`  
  Configure from appsettings.json or environment variables.

### Behavior Methods

- `void AddAsService(IServiceCollection serviceCollection, ServiceLifetime lifetime)`  
  Register the logger as a service in the DI container.

- `TLogger Build()`  
  Build the logger.

- `TLoggerConfiguration BuildLoggerConfiguration()`  
  Build only the configuration.

- `void Reset()`  
  Reset all configuration.

---

## 💡 Example

```csharp
var logger = new SerilogBuilder()
    .OutputToFile(true, "logs", "myapp", "log")
    .WithMinimumLevel(LoggingLevel.Information)
    .WithOutputTemplate("{Timestamp} [{Level}] {Message}{NewLine}{Exception}")
    .Build();
```

---

## 🧪 Testing Strategy

This interface is intended to be used by multiple logger implementations (e.g. Serilog, NLog). Unit test coverage should validate:

- Fluent method chains
- Configuration delegation and overrides
- Integration with `IServiceCollection`
