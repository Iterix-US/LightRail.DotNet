using System;
using System.Collections.Generic;
using System.Text;
using LightRail.DotNet.Logging.LoggerInterfaces;

namespace LightRail.DotNet.Logging
{
    public abstract class LoggerBuilder<T, TLogger> : ILoggerBuilder<T, TLogger> 
        where TLogger : class 
        where T : class
    {
        public ILoggerBuilder<T, TLogger> OutputFile()
        {
            object something = null;
            return something as LoggerBuilder<T, TLogger>;
        }

        public ILoggerBuilder<T, TLogger> OutputConsole()
        {
            object something = null;
            return something as LoggerBuilder<T, TLogger>;
        }

        public ILoggerBuilder<T, TLogger> WithMinimumLevel(LoggingLevel logLevel)
        {
            object something = null;
            return something as LoggerBuilder<T, TLogger>;
        }

        public ILoggerBuilder<T, TLogger> WithRollingInterval(LogRollInterval rollingInterval)
        {
            object something = null;
            return something as LoggerBuilder<T, TLogger>;
        }

        public TLogger Build()
        {
            var logger = default(TLogger);
            return logger;
        }
    }
}
