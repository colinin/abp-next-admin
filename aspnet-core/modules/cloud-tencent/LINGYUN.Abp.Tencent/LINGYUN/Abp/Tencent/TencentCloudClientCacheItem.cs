using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Tencent;

public class TencentCloudClientCacheItem
{
    public const string CacheKeyFormat = "pn:{0},n:tenant-cloud";
    public string SecretId { get; set; }
    public string SecretKey { get; set; }
    public string EndPoint { get; set; }
    public string WebProxy { get; set; }
    public string ApiEndPoint { get; set; }
    public string HttpMethod { get; set; }
    public int Timeout { get; set; }
    public int DurationSecond { get; set; }

    public static string CalculateCacheKey(ICurrentTenant currentTenant)
    {
        return string.Format(CacheKeyFormat, currentTenant.IsAvailable ? currentTenant.GetId().ToString() : "global");
    }
}
