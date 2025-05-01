namespace LightRail.DotNet.Logging.LoggerInterfaces
{
    public interface ILoggerBuilder<out TLoggerBuilder, out TLogger, out TLoggerConfiguration> 
        where TLoggerBuilder : class 
        where TLogger : class
        where TLoggerConfiguration : class
    {
        TLoggerConfiguration Configuration { get; }

        /// <summary>
        /// Outputs log entries to a file.
        /// </summary>
        /// <param name="createLogPath">True: if the directory does not exist, create it</param>
        /// <param name="logPath">The directory, file name, and file extension to be used for the log (e.g. C:\Some\Directory\SomeLog.log)</param>
        /// <returns>The current snapshot of the builder</returns>
        TLoggerBuilder OutputToFile(bool createLogPath = false, string logPath = null);

        /// <summary>
        /// Outputs log entries to the console
        /// </summary>
        /// <returns></returns>
        TLoggerBuilder OutputToConsole();
        TLoggerBuilder WithMinimumLevel(LoggingLevel logLevel);
        TLoggerBuilder WithRollingInterval(LogRollInterval rollingInterval);
        TLoggerBuilder WithFileSizeLimit(long fileSizeLimit);
        TLoggerBuilder WithFileRetentionLimit(int fileCount);
        TLoggerBuilder WithOutputTemplate(string outputTemplate);
        TLogger Build(string category = null);
        TLoggerConfiguration BuildLoggerConfiguration();
    }
}
