using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.Saas.Tenants;

public class TenantConnectionStringCheckInput
{
    [Required]
    public string Provider { get; set; }

    public string Name { get; set; }

    [Required]
    [DisableAuditing]
    [DynamicStringLength(typeof(TenantConnectionStringConsts), nameof(TenantConnectionStringConsts.MaxValueLength))]
    public string ConnectionString { get; set; }
}
