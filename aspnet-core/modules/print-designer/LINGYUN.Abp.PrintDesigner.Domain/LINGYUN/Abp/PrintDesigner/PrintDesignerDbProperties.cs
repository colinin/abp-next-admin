using Volo.Abp.Data;

namespace LINGYUN.Abp.PrintDesigner;

public static class PrintDesignerDbProperties
{
    public static string DbTablePrefix { get; set; } = AbpCommonDbProperties.DbTablePrefix + "PrintDesigner";

    public static string? DbSchema { get; set; } = AbpCommonDbProperties.DbSchema;

    public const string ConnectionStringName = "AbpPrintDesigner";
}
