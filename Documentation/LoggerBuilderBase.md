# LoggerBuilderBase&lt;TLoggerBuilder, TLogger, TConfiguration&gt;

Provides a base implementation of a strongly-typed, extensible logging builder interface for both Serilog and NLog, implementing shared logging configuration and logic. Inherit from this class to create custom logger builders that define specific behavior for your preferred logging provider.

---

## 📦 Namespace

`SeroGlint.DotNet.Logging`

## 🧰 Inheritance

Implements: `ILoggerBuilder<TLoggerBuilder, TLogger, TConfiguration>`

---

## ⚙️ Defaults

| Property                   | Default Value                                                                 |
|----------------------------|--------------------------------------------------------------------------------|
| Output to Console          | false                                                                          |
| Output to File             | false                                                                          |
| Log Path                   | `AppContext.BaseDirectory`                                                     |
| Log File Name              | `"AppLog"`                                                                     |
| Log File Extension         | `"log"`                                                                        |
| Log File Location          | `${LogPath}/${LogName}.${LogExtension}`                                       |
| Create Log Path            | false                                                                          |
| Category                   | `Assembly.GetExecutingAssembly().GetName().Name ?? "AppLogger"`                |
| Override Default Config    | false                                                                          |
| Log File Count Limit       | 31                                                                             |
| Log File Size Limit        | 10 MB                                                                          |
| Log Rolling Interval       | `LogRollInterval.Monthly`                                                      |
| Log Level                  | `LoggingLevel.Debug`                                                           |
| Output Template            | `{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}`|

---

## 🔧 Inheritable Behavior

Classes that derive from this base must implement:

- `Build()` - Builds and returns the concrete logger instance.
- `BuildLoggerConfiguration()` - Builds the final logger configuration object.
- `OutputToFile()` - Enables and configures file logging.
- `OutputToConsole()` - Enables console logging.
- `WithMinimumLevel()` - Sets the log level.
- `WithRollingInterval()` - Sets the log file rolling interval.
- `WithFileSizeLimit()` - Sets the file size limit.
- `WithFileRetentionLimit()` - Sets how many files to keep.
- `WithOutputTemplate()` - Sets the output formatting template.
- `WithConfiguration()` - Passes custom configuration.
- `WithEnhancement()` - Allows custom transformations on the config.
- `WithCategory()` - Groups the logger by purpose/module.
- `AddAsService()` - Registers the logger with DI.
- `FromConfiguration()` - Reads config values from an IConfiguration source.
- `Reset()` - Resets the builder to defaults.

---

## 📘 Remarks

This base class encapsulates common logic and sensible defaults that eliminate redundant boilerplate when configuring loggers. It empowers derived implementations to focus solely on the provider-specific logic (e.g., Serilog or NLog).
