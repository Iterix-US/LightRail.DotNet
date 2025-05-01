using LightRail.DotNet.Logging.LoggerInterfaces;
using Serilog;

namespace LightRail.DotNet.Logging
{
    /// <summary>
    /// Defaults:
    ///     Output to console: false
    ///     Output to file: false
    ///     Log path: Empty
    ///     Log file count limit: 31
    ///     Log file size limit: 10 MB
    ///     Log rolling interval: Monthly
    ///     Log level: Debug
    ///     Log output template: {Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}
    /// </summary>
    public abstract class LoggerBuilderBase<TLoggerBuilder, TLogger, TConfiguration> : ILoggerBuilder<TLoggerBuilder, TLogger, TConfiguration> 
        where TLoggerBuilder : class 
        where TLogger : class 
        where TConfiguration : class
    {
        public abstract TConfiguration Configuration { get; }
        protected LoggingLevel LogLevel { get; set; } = LoggingLevel.Debug;
        protected LogRollInterval RollingInterval { get; set; } = LogRollInterval.Monthly;
        protected string LogPath { get; set; } = string.Empty;
        protected string OutputTemplate { get; set; } = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}";
        protected bool EnableConsoleOutput { get; set; }
        protected bool EnableFileOutput { get; set; }
        protected int RetainedFileCountLimit { get; set; } = 31;
        protected long FileSizeLimit { get; set; } = 10485760; // 10 MB

        public abstract TLoggerBuilder OutputToFile(bool createLogPath = false, string logPath = null);
        public abstract TLoggerBuilder OutputToConsole();
        public abstract TLoggerBuilder WithMinimumLevel(LoggingLevel logLevel);
        public abstract TLoggerBuilder WithRollingInterval(LogRollInterval rollingInterval);
        public abstract TLoggerBuilder WithFileSizeLimit(long fileSizeLimit);
        public abstract TLoggerBuilder WithFileRetentionLimit(int fileCount);
        public abstract TLoggerBuilder WithOutputTemplate(string outputTemplate);
        public abstract TLogger Build(string category = null);
        public abstract TConfiguration BuildLoggerConfiguration();
    }
}
