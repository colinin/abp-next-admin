using LINGYUN.Abp.IdGenerator.Snowflake;

namespace LINGYUN.Abp.Serilog.Enrichers.UniqueId;

public class AbpSerilogEnrichersUniqueIdOptions
{
    public SnowflakeIdOptions SnowflakeIdOptions { get; set; }
    public AbpSerilogEnrichersUniqueIdOptions()
    {
        SnowflakeIdOptions = new SnowflakeIdOptions();
    }
}
