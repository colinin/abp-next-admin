using Volo.Abp.Data;

namespace LINGYUN.Abp.TextTemplating;

public class AbpTextTemplatingDbProperties
{
    public static string DbTablePrefix { get; set; } = AbpCommonDbProperties.DbTablePrefix;

    public static string DbSchema { get; set; } = AbpCommonDbProperties.DbSchema;

    public const string ConnectionStringName = "AbpTextTemplating";
}
