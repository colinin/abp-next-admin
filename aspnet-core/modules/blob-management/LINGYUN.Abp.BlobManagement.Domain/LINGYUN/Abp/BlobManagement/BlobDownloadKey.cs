using System;

namespace LINGYUN.Abp.BlobManagement;

/// <summary>
/// 下载密钥承载的数据
/// </summary>
public class BlobDownloadKey
{
    /// <summary>
    /// Blob标识
    /// </summary>
    public Guid BlobId { get; }
    /// <summary>
    /// 租户标识(宿主为null)
    /// </summary>
    public Guid? TenantId { get; }
    /// <summary>
    /// 过期时间(UTC)
    /// </summary>
    public DateTime ExpiresAt { get; }

    public BlobDownloadKey(Guid blobId, Guid? tenantId, DateTime expiresAt)
    {
        BlobId = blobId;
        TenantId = tenantId;
        ExpiresAt = expiresAt;
    }
}
