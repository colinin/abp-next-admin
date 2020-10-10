using Volo.Abp.Data;

namespace LINGYUN.Abp.RulesManagement
{
    public static class RulesManagementDbProperties
    {
        public static string DbTablePrefix { get; set; } = "App";

        public static string DbSchema { get; set; } = AbpCommonDbProperties.DbSchema;

        public const string ConnectionStringName = "AppRulesManagement";
    }
}
