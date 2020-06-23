using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using System;
using Volo.Abp;

namespace LINGYUN.Abp.ExceptionHandling
{
    public class ExceptionSendNotifierContext
    {
        [NotNull]
        public Exception Exception { get; }

        [NotNull]
        public IServiceProvider ServiceProvider { get; }

        public LogLevel LogLevel { get; }
        internal ExceptionSendNotifierContext(
            [NotNull]  IServiceProvider serviceProvider,
            [NotNull] Exception exception,
            LogLevel? logLevel = null)
        {
            ServiceProvider = Check.NotNull(serviceProvider, nameof(serviceProvider));
            Exception = Check.NotNull(exception, nameof(exception));
            LogLevel = logLevel ?? exception.GetLogLevel();
        }
    }
}
