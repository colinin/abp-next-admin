using System;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.BlobManagement;

public class BlobEto : IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid Id { get; set; }
    public BlobEto()
    {

    }

    public BlobEto(Guid id, Guid? tenantId = null)
    {
        Id = id;
        TenantId = tenantId;
    }
}
