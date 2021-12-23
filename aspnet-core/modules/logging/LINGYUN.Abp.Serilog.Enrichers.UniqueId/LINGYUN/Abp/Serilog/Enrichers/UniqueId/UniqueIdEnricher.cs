using LINGYUN.Abp.IdGenerator;
using LINGYUN.Abp.IdGenerator.Snowflake;
using Serilog.Core;
using Serilog.Events;

namespace LINGYUN.Abp.Serilog.Enrichers.UniqueId
{
    public class UniqueIdEnricher : ILogEventEnricher
    {
        private readonly static IDistributedIdGenerator _distributedIdGenerator
            = SnowflakeIdGenerator.Create(new SnowflakeIdOptions());

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddOrUpdateProperty(
                propertyFactory.CreateProperty(
                    AbpSerilogUniqueIdConsts.UniqueIdPropertyName,
                    _distributedIdGenerator.Create()));
        }
    }
}
