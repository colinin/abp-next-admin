using System;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.BlobManagement;

public class BlobDownloadEto : IMultiTenant
{
    public Guid? TenantId { get; set; }
    public string ContainerName { get; set; }
    public string FullName { get; set; }
    public BlobDownloadEto()
    {

    }

    public BlobDownloadEto(string containerName, string fullName, Guid? tenantId = null)
    {
        ContainerName = containerName;
        FullName = fullName;
        TenantId = tenantId;
    }
}
