using System;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.BlobManagement;

public class BlobContainerEto : IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid Id { get; set; }
    public BlobContainerEto()
    {

    }

    public BlobContainerEto(Guid id, Guid? tenantId = null)
    {
        Id = id;
        TenantId = tenantId;
    }
}
