using Serilog.Core;
using Serilog.Events;

namespace LINGYUN.Abp.Serilog.Enrichers.Application
{
    public class ApplicationNameEnricher : ILogEventEnricher
    {
        LogEventProperty _cachedProperty;
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(GetLogEventProperty(propertyFactory));
        }

        private LogEventProperty GetLogEventProperty(ILogEventPropertyFactory propertyFactory)
        {
            if (_cachedProperty == null)
                _cachedProperty = CreateProperty(propertyFactory);

            return _cachedProperty;
        }

        private static LogEventProperty CreateProperty(ILogEventPropertyFactory propertyFactory)
        {
            return propertyFactory.CreateProperty(
                AbpSerilogEnrichersConsts.ApplicationNamePropertyName,
                AbpSerilogEnrichersConsts.ApplicationName);
        }
    }
}
