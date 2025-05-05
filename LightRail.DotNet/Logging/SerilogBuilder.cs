using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using LightRail.DotNet.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace LightRail.DotNet.Logging
{
    // Tested via <see cref="LoggerFactoryBuilder"/>, so the derived logger class doesn't need testing directly.
    [ExcludeFromCodeCoverage]
    public class SerilogBuilder : LoggerBuilderBase<SerilogBuilder, ILogger, LoggerConfiguration>
    {
        public override LoggerConfiguration Configuration { get; internal set; } = new LoggerConfiguration();

        public override SerilogBuilder OutputToFile(bool createLogPath = false, string logPath = null, string logName = null, string logExtension = null)
        {
            EnableFileOutput = true;
            CreateLogPath = createLogPath;

            LogFilePath = string.IsNullOrWhiteSpace(logPath) ? AppContext.BaseDirectory : logPath;
            LogFileName = string.IsNullOrWhiteSpace(logName) ? "Log" : logName;
            LogFileExtension = string.IsNullOrWhiteSpace(logExtension?.TrimStart('.')) ? "log" : logExtension.TrimStart('.');

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

        public override SerilogBuilder WithConfiguration(LoggerConfiguration configuration)
        {
            OverrideDefaultConfiguration = true;
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration), "Logger configuration cannot be null.");
            return this;
        }

        public override SerilogBuilder WithEnhancement(Func<LoggerConfiguration, LoggerConfiguration> enhancement)
        {
            if (enhancement == null)
            {
                throw new ArgumentNullException(nameof(enhancement), "Enhancement function cannot be null");
            }

            Configuration = enhancement(Configuration);
            return this;
        }

        public override SerilogBuilder WithCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                throw new ArgumentException("Category cannot be null or whitespace.", nameof(category));
            }

            Category = category;

            return this;
        }

        public override void AddAsService(IServiceCollection serviceCollection, ServiceLifetime lifetime)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection), "Service collection cannot be null.");
            }

            if (!IsBuilt)
            {
                Build();
            }

            serviceCollection.AddLogging(logging => logging.AddSerilog());

            switch (lifetime)
            {
                case ServiceLifetime.Scoped:
                    serviceCollection.AddScoped(_ => Instance);
                    break;
                case ServiceLifetime.Singleton:
                    serviceCollection.AddSingleton(_ => Instance);
                    break;
                case ServiceLifetime.Transient:
                    serviceCollection.AddTransient(_ => Instance);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lifetime), "Unrecognized lifetime selection.");
            }
        }

        public override SerilogBuilder FromConfiguration(IConfiguration configuration)
        {
            Configuration.ReadFrom.Configuration(configuration);
            OverrideDefaultConfiguration = true;
            return this;
        }

        public override void Reset()
        {
            Configuration = new LoggerConfiguration();
            Instance = null;
            IsBuilt = false;
            OverrideDefaultConfiguration = false;
        }

        public override LoggerConfiguration BuildLoggerConfiguration()
        {
            if (OverrideDefaultConfiguration)
            {
                return Configuration;
            }

            if (EnableFileOutput)
            {
                Configuration.WriteTo.File(
                    LogFileLocation,
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

        public override ILogger Build()
        {
            if (Instance != null)
            {
                return Instance;
            }

            if (IsBuilt)
            {
                throw new InvalidOperationException("Logger has already been built. Create a new instance of the logger to build anew.");
            }

            if (CreateLogPath && !Directory.Exists(LogFilePath))
            {
                Directory.CreateDirectory(LogFilePath);
            }

            BuildLoggerConfiguration();

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                Log.Logger = Configuration.CreateLogger();
                builder.AddSerilog();
            });

            var effectiveCategory = Category ?? Assembly.GetExecutingAssembly().GetName().Name ?? "AppLogger";
            var logger = loggerFactory.CreateLogger(effectiveCategory);

            const string loggerInitializedMessage = "Logger initialized with Serilog";
            logger.LogInformation(loggerInitializedMessage);
            logger.LogDebug(loggerInitializedMessage);

            IsBuilt = true;
            Instance = logger;

            if (EnableFileOutput || EnableConsoleOutput)
            {
                Instance.LogInformation($"Logger location: {LogFileLocation}");
            }

            return logger;
        }
    }
}
