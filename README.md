# SeroGlint.DotNet

A modular collection of utilities, interfaces, and service builders designed to simplify .NET development for cross-cutting concerns like logging, configuration, encryption, and messaging.

---

## 📚 Projects and Documentation

Each major component is documented with its own markdown file under the `Documentation` directory.

| Component                            | Description                                                              | Documentation File                                           |
|--------------------------------------|--------------------------------------------------------------------------|--------------------------------------------------------------|
| `ConfigurationLoader`               | Loads configuration files with support for mocks and tests.             | [ConfigurationLoader.md](Documentation/ConfigurationLoader.md) |
| `ILoggerBuilder`                    | Provides extensible logging setup with Serilog/NLog.                    | [ILoggerBuilder.md](Documentation/ILoggerBuilder.md)             |
| `PasswordUtility`                   | Secure password hashing and validation helper.                          | [PasswordUtility.md](Documentation/PasswordUtility.md)           |
| `AesEncryptionService`              | AES-256 encryption wrapper with logging and IV derivation.              | [AesEncryptionService.md](Documentation/AesEncryptionService.md) |
| `PipeEnvelope<T>`                   | Represents a typed, serializable message envelope.                      | [PipeEnvelope.md](Documentation/PipeEnvelope.md)                 |
| `NamedPipeClient`                   | Sends encrypted or plain messages over named pipes.                     | [NamedPipeClient.md](Documentation/NamedPipeClient.md)           |
| `NamedPipeServerCore`              | Listens for connections and handles message routing.                    | [NamedPipeServerCore.md](Documentation/NamedPipeServerCore.md)   |
| `PipeMessageReceivedEventArgs`     | Event args used when a message is received on the server.               | [PipeMessageReceivedEventArgs.md](Documentation/PipeMessageReceivedEventArgs.md) |
| `PipeResponseRequestedEventArgs`   | Event args used when a response is triggered by the server.             | [PipeResponseRequestedEventArgs.md](Documentation/PipeResponseRequestedEventArgs.md) |
| `INamedPipeClient`                 | Interface abstraction for the named pipe client.                        | [INamedPipeClient.md](Documentation/INamedPipeClient.md)         |
| `INamedPipeServerCore`            | Interface abstraction for the server core.                              | [INamedPipeServerCore.md](Documentation/INamedPipeServerCore.md) |
| `INamedPipeConfigurator`          | Interface describing the pipe config (e.g., server, pipe name, encryption). | [NamedPipeConfigurator.md](Documentation/NamedPipeConfigurator.md) |

---

## 📦 NuGet Dependencies

These packages are required when using `SeroGlint.DotNet`:

| Package                                  | Version | Purpose                                                                 |
|------------------------------------------|---------|-------------------------------------------------------------------------|
| `Microsoft.Extensions.Logging.Abstractions` | 9.0.4   | Standard logging interfaces for dependency injection and mockability    |
| `NLog`                                   | 5.4.0   | Core NLog logging engine                                                |
| `NLog.Extensions.Hosting`                | 5.4.0   | Integration with `IHostBuilder`                                         |
| `NLog.Extensions.Logging`                | 5.4.0   | Connects NLog to `ILogger`                                              |
| `NLog.Web.AspNetCore`                    | 5.4.0   | ASP\.NET Core integration for NLog                                       |
| `Serilog`                                | 4.2.0   | Core Serilog logging engine                                             |
| `Serilog.AspNetCore`                     | 9.0.0   | ASP\.NET Core-specific extensions                                        |
| `Serilog.Extensions.Hosting`             | 9.0.0   | Hosting integration                                                     |
| `Serilog.Sinks.Console`                  | 6.0.0   | Console logging for Serilog                                             |
| `Serilog.Sinks.Debug`                    | 3.0.0   | Debug output sink                                                       |
| `Serilog.Sinks.File`                     | 7.0.0   | File logging output                                                     |
| `System.Security.Cryptography.Algorithms`| 4.3.1   | Enables AES encryption in secure communication                         |
| `System.Text.Json`                       | 9.0.3   | JSON (de)serialization used across pipe messaging and utilities         |


## 🧪 Testing

Unit tests are written using `xUnit` and `NSubstitute`, following the Arrange-Act-Assert pattern. Most components are fully mockable and testable due to interface abstractions.

Run all tests:

```bash
dotnet test
```

---

## 🔐 Security & Reliability

- Internal helpers (like `AesEncryptionService`) isolate security logic from consumer code.
- Named pipe handlers include cancellation support and structured error logging.
- All I/O logic is abstracted for testability.

---

## 🤝 Contributions

Contributions are welcome! Please follow the [guidelines](CONTRIBUTING.md) and submit PRs with clear descriptions and passing tests.

---

© Iterix · Built for performance, extensibility, and clarity