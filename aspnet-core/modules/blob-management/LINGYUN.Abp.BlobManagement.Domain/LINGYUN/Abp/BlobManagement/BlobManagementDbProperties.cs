using Volo.Abp.Data;

namespace LINGYUN.Abp.BlobManagement;

public class BlobManagementDbProperties
{
    public static string DbTablePrefix { get; set; } = AbpCommonDbProperties.DbTablePrefix;

    public static string? DbSchema { get; set; } = AbpCommonDbProperties.DbSchema;

    public const string ConnectionStringName = "AbpBlobManagement";
}
