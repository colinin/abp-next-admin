using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.Saas.Tenants;
public class TenantUpdateDto : TenantCreateOrUpdateBase, IHasConcurrencyStamp
{
    public string ConcurrencyStamp { get; set; }
}