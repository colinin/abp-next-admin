using Volo.Abp.Data;

namespace LINGYUN.Abp.AIManagement;
public static class AbpAIManagementDbProperties
{
    public static string DbTablePrefix { get; set; } = AbpCommonDbProperties.DbTablePrefix + "AI";

    public static string? DbSchema { get; set; } = AbpCommonDbProperties.DbSchema;

    public const string ConnectionStringName = "AbpAIManagement";
}
