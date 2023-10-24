namespace LINGYUN.Abp.Tencent;

public class TencentCloudClientCacheItem
{
    public const string CacheKeyFormat = "pn:tenant-cloud,n:{0}";
    public string SecretId { get; set; }
    public string SecretKey { get; set; }
    public string EndPoint { get; set; }
    public string WebProxy { get; set; }
    public string ApiEndPoint { get; set; }
    public string HttpMethod { get; set; }
    public int Timeout { get; set; }
    public int DurationSecond { get; set; }

    public static string CalculateCacheKey(string provider)
    {
        return string.Format(CacheKeyFormat, provider);
    }
}
