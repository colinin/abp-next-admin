using System;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.BlobManagement;

[IgnoreMultiTenancy]
public class BlobDownloadKeyCacheItem
{
    public string Url { get; set; }
    public Guid BlobId { get; set; }
    public Guid? TenantId { get; set; }
    public BlobDownloadKeyCacheItem()
    {

    }

    public BlobDownloadKeyCacheItem(Guid blobId, string url, Guid? tenantId = null)
    {
        BlobId = blobId;
        Url = url;
        TenantId = tenantId;
    }
}
