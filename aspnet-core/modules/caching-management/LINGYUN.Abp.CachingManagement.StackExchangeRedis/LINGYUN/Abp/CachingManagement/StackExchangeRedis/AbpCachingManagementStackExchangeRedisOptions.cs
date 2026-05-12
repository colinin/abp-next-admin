namespace LINGYUN.Abp.CachingManagement.StackExchangeRedis;

public class AbpCachingManagementStackExchangeRedisOptions
{
    public int MaxScanDept { get; set; }
    public int ScanCount { get; set; }
    public AbpCachingManagementStackExchangeRedisOptions()
    {
        MaxScanDept = 20;
        ScanCount = 10000;
    }
}
