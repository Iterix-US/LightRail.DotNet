using System;
using System.IO;
using LightRail.DotNet.Extensions;
using LightRail.DotNet.Logging.LoggerInterfaces;
using Microsoft.Extensions.Logging;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace LightRail.DotNet.Logging
{
    public class SerilogBuilder : LoggerBuilderBase<SerilogBuilder, ILogger, LoggerConfiguration>
    {
        public override LoggerConfiguration Configuration { get; } = new LoggerConfiguration();

        public override SerilogBuilder OutputToFile(bool createLogPath = false, string logPath = null)
        {
            EnableFileOutput = true;
            LogPath = logPath ?? Path.Combine(AppContext.BaseDirectory, "Logs");
            return this;
        }

        public override SerilogBuilder OutputToConsole()
        {
            EnableConsoleOutput = true;
            return this;
        }

        public override SerilogBuilder WithMinimumLevel(LoggingLevel logLevel)
        {
            LogLevel = logLevel;
            return this;
        }

        public override SerilogBuilder WithRollingInterval(LogRollInterval rollingInterval)
        {
            RollingInterval = rollingInterval;
            return this;
        }

        public override SerilogBuilder WithFileSizeLimit(long fileSizeLimit)
        {
            FileSizeLimit = fileSizeLimit;
            return this;
        }

        public override SerilogBuilder WithFileRetentionLimit(int fileCount)
        {
            RetainedFileCountLimit = fileCount;
            return this;
        }

        public override SerilogBuilder WithOutputTemplate(string outputTemplate)
        {
            OutputTemplate = outputTemplate;
            return this;
        }

        public override ILogger Build(string category = null)
        {
            if (!Directory.Exists(LogPath))
            {
                Directory.CreateDirectory(LogPath);
            }

            BuildLoggerConfiguration();

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog();
                Log.Logger = Configuration.CreateLogger();
            });

            var logger = loggerFactory.CreateLogger(category ?? "SerilogLogger");
            logger.LogInformation("Logger initialized with Serilog");
            return logger;
        }

        public override LoggerConfiguration BuildLoggerConfiguration()
        {
            if (EnableFileOutput)
            {
                Configuration.WriteTo.File(
                    LogPath,
                    rollingInterval: RollingInterval.ToSerilogRollingInterval(),
                    fileSizeLimitBytes: FileSizeLimit,
                    retainedFileCountLimit: RetainedFileCountLimit,
                    rollOnFileSizeLimit: true,
                    outputTemplate: OutputTemplate);
            }

            if (EnableConsoleOutput)
            {
                Configuration.WriteTo.Console(
                    outputTemplate: OutputTemplate,
                    theme: Serilog.Sinks.SystemConsole.Themes.SystemConsoleTheme.Literate
                );
            }

            Configuration.MinimumLevel.Is(LogLevel.ToSerilogLevel());

            return Configuration;
        }
    }
}
