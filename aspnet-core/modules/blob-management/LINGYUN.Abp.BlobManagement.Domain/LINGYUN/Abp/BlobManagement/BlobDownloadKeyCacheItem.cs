using System;

namespace LINGYUN.Abp.BlobManagement;

public class BlobDownloadKeyCacheItem
{
    public string Url { get; set; }
    public Guid BlobId { get; set; }
    public BlobDownloadKeyCacheItem()
    {

    }

    public BlobDownloadKeyCacheItem(Guid blobId, string url)
    {
        BlobId = blobId;
        Url = url;
    }

    public static string CalculateCacheKey(Guid blobId, string blobName)
    {
        return $"{blobId:N}{blobName.RemovePreFix("/")}";
    }
}
