using System;
using LightRail.DotNet.Logging.LoggerInterfaces;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace LightRail.DotNet.Logging
{
    public class NLogBuilder : ILoggerBuilder<NLogBuilder, ILogger, NLogLoggingConfiguration>
    {
        public NLogLoggingConfiguration Configuration { get; } = null;

        public NLogBuilder OutputFile()
        {
            throw new NotImplementedException();
        }

        public NLogBuilder OutputConsole()
        {
            throw new NotImplementedException();
        }

        public NLogBuilder WithMinimumLevel(LoggingLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public NLogBuilder WithRollingInterval(LogRollInterval rollingInterval)
        {
            throw new NotImplementedException();
        }

        public ILogger Build()
        {
            throw new NotImplementedException();
        }

        public NLogLoggingConfiguration BuilderLoggerConfiguration()
        {
            throw new NotImplementedException();
        }
    }
}
