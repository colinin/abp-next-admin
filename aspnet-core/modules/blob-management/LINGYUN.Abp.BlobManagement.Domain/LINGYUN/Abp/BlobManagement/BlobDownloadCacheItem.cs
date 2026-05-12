namespace LINGYUN.Abp.BlobManagement;

public class BlobDownloadCacheItem
{
    private const string CacheKeyFormat = "cn:{0};bn:{1}";
    public long DownloadCount { get; set; }
    public BlobDownloadCacheItem()
    {

    }

    public BlobDownloadCacheItem(long downloadCount)
    {
        DownloadCount = downloadCount;
    }

    public static string CalculateCacheKey(string containerName, string blobName)
    {
        return string.Format(CacheKeyFormat, containerName, blobName);
    }
}
