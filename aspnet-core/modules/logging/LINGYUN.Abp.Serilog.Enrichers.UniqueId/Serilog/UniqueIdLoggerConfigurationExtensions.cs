using LINGYUN.Abp.Serilog.Enrichers.UniqueId;
using Serilog.Configuration;
using System;

namespace Serilog
{
    public static class UniqueIdLoggerConfigurationExtensions
    {
        public static LoggerConfiguration WithUniqueId(
           this LoggerEnrichmentConfiguration enrichmentConfiguration)
        {
            if (enrichmentConfiguration == null) throw new ArgumentNullException(nameof(enrichmentConfiguration));
            return enrichmentConfiguration.With<UniqueIdEnricher>();
        }
    }
}
