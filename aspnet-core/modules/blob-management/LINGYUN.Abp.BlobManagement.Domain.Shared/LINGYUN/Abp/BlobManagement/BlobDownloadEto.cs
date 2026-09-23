using System;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.BlobManagement;

public class BlobDownloadEto : IMultiTenant
{
    public Guid? TenantId { get; set; }
    public string Provider { get; set; } = default!;
    public string ContainerName { get; set; } = default!;
    public string FullName { get; set; } = default!;
    public BlobDownloadEto()
    {

    }

    public BlobDownloadEto(
        string provider, 
        string containerName,
        string fullName,
        Guid? tenantId = null)
    {
        Provider = provider;
        ContainerName = containerName;
        FullName = fullName;
        TenantId = tenantId;
    }
}
