using LINGYUN.Abp.Serilog.Enrichers.Application;
using LINGYUN.Abp.Serilog.Enrichers.UniqueId;
using System;

namespace LINGYUN.Abp.Logging.Serilog.Elasticsearch
{
    public class SerilogField
    {
        [Nest.PropertyName(AbpSerilogUniqueIdConsts.UniqueIdPropertyName)]
        public long UniqueId { get; set; }

        [Nest.PropertyName(AbpLoggingEnricherPropertyNames.MachineName)]
        public string MachineName { get; set; }

        [Nest.PropertyName(AbpLoggingEnricherPropertyNames.EnvironmentName)]
        public string Environment { get; set; }

        [Nest.PropertyName(AbpSerilogEnrichersConsts.ApplicationNamePropertyName)]
        public string Application { get; set; }

        [Nest.PropertyName("SourceContext")]
        public string Context { get; set; }

        [Nest.PropertyName("ActionId")]
        public string ActionId { get; set; }

        [Nest.PropertyName("ActionName")]
        public string ActionName { get; set; }

        [Nest.PropertyName("RequestId")]
        public string RequestId { get; set; }

        [Nest.PropertyName("RequestPath")]
        public string RequestPath { get; set; }

        [Nest.PropertyName("ConnectionId")]
        public string ConnectionId { get; set; }

        [Nest.PropertyName("CorrelationId")]
        public string CorrelationId { get; set; }

        [Nest.PropertyName("ClientId")]
        public string ClientId { get; set; }

        [Nest.PropertyName("UserId")]
        public string UserId { get; set; }

        [Nest.PropertyName("TenantId")]
        public Guid? TenantId { get; set; }

        [Nest.PropertyName("ProcessId")]
        public int ProcessId { get; set; }

        [Nest.PropertyName("ThreadId")]
        public int ThreadId { get; set; }
    }
}
