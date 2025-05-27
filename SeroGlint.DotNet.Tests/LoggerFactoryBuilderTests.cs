using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog;
using NSubstitute;
using SeroGlint.DotNet.Abstractions;
using SeroGlint.DotNet.Logging;
using SeroGlint.DotNet.Tests.Utilities;

namespace SeroGlint.DotNet.Tests
{
    public class LoggerFactoryBuilderTests : IDisposable
    {
        private readonly LogFileCleaner _cleaner = new();
        private const string TestLogPath = @".\tmp\log_test_tagsw40k";

        public LoggerFactoryBuilderTests()
        {
            if (!Directory.Exists(TestLogPath))
            {
                Directory.CreateDirectory(TestLogPath);
            }
        }

        void IDisposable.Dispose()
        {
            Thread.Sleep(100);
            _cleaner.TryDelete(FindFile());
        }

        private static string FindFile()
        {
            return Directory
                .GetFiles(TestLogPath, "TestLog*.log")
                .FirstOrDefault() ?? string.Empty;
        }

        [Fact]
        public void BuildSerilog_CreatesLogger_WithFileAndConsole()
        {
            var builder = new LoggerFactoryBuilder()
                .EnableConsoleOutput()
                .EnableFileOutput(createLogPath: true, logPath: TestLogPath, logName: "TestLog", logExtension: "log")
                .SetMinimumLevel(LoggingLevel.Information)
                .SetRollingInterval(LogRollInterval.Monthly)
                .SetFileSizeLimit(5 * 1024 * 1024)
                .SetFileRetentionLimit(5)
                .SetOutputTemplate("{Timestamp:HH:mm:ss} {Message}{NewLine}")
                .SetCategory("TestCategory");

            var logger = builder.BuildSerilog();

            Assert.NotNull(logger);
            logger.LogInformation("This is a test log message.");

            Serilog.Log.CloseAndFlush();
            Thread.Sleep(100);

            Assert.True(File.Exists(FindFile()), "Expected log file was not created.");
            var contents = File.ReadAllText(FindFile());
            Assert.Contains("This is a test log message", contents);
        }

        [Fact]
        public void BuildNLog_CreatesLogger_WithFileAndConsole()
        {
            // Arrange
            var builder = new LoggerFactoryBuilder()
                .EnableConsoleOutput()
                .EnableFileOutput(createLogPath: true, logPath: TestLogPath, logName: "TestLog", logExtension: "log")
                .SetMinimumLevel(LoggingLevel.Information)
                .SetRollingInterval(LogRollInterval.Monthly)
                .SetFileSizeLimit(5 * 1024 * 1024)
                .SetFileRetentionLimit(5)
                .SetOutputTemplate("{longdate} {message}{newline}")
                .SetCategory("TestCategory");

            // Act
            var logger = builder.BuildNLog();
            Assert.NotNull(logger);
            logger.LogInformation("This is a test log message from NLog.");
            LogManager.Flush();
            Thread.Sleep(100);

            // Assert
            Assert.True(File.Exists(FindFile()), "Expected log file was not created.");
        }

        [Fact]
        public void BuildSerilog_WithConfiguration_UsesConfigSettings()
        {
            var configBuilder = new ConfigurationBuilder()
                .AddInMemoryCollection([
                    new KeyValuePair<string, string?>("Serilog:MinimumLevel:Default", "Warning")
                ]);

            var configuration = configBuilder.Build();

            var builder = new LoggerFactoryBuilder()
                .EnableConsoleOutput()
                .EnableFileOutput(createLogPath: true, logPath: TestLogPath, logName: "TestLog", logExtension: "log")
                .SetConfiguration(configuration);

            var logger = builder.BuildSerilog();

            Assert.NotNull(logger);
        }

        [Fact]
        public void BuildNLog_WithConfiguration_UsesConfigSettings()
        {
            var configBuilder = new ConfigurationBuilder()
                .AddInMemoryCollection([
                    new KeyValuePair<string, string?>("NLog:targets:file:type", "File"),
                    new KeyValuePair<string, string?>("NLog:targets:file:fileName", "TestLog.log"),
                    new KeyValuePair<string, string?>("NLog:targets:file:layout", "{message}"),
                    new KeyValuePair<string, string?>("NLog:rules:0:logger", "*"),
                    new KeyValuePair<string, string?>("NLog:rules:0:minLevel", "Info"),
                    new KeyValuePair<string, string?>("NLog:rules:0:writeTo", "file")
                ]);

            var configuration = configBuilder.Build();

            var builder = new LoggerFactoryBuilder()
                .SetConfiguration(configuration);

            var logger = builder.BuildNLog();

            Assert.NotNull(logger);
        }

        [Fact]
        public void FileManager_Property_ReturnsInjectedInstance()
        {
            // Arrange
            var mockManager = Substitute.For<IDirectoryManagement>();
            var loggerBuilder = new LoggerFactoryBuilder(mockManager)
                .EnableConsoleOutput()
                .EnableFileOutput(createLogPath: true, logPath: TestLogPath, logName: "TestLog", logExtension: "log");

            var actual = loggerBuilder.FileManager;

            // Assert
            Assert.NotNull(actual);
        }
    }
}
