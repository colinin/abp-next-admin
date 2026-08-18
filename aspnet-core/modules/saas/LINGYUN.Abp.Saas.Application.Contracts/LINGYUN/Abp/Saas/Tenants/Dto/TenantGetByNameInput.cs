using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.Saas.Tenants;

public class TenantGetByNameInput
{
    [Required]
    [DynamicStringLength(typeof(TenantConsts), nameof(TenantConsts.MaxNameLength))]
    public string Name { get; set; } = default!;

    public TenantGetByNameInput() { }
    public TenantGetByNameInput(string name)
    {
        Name = name;
    }
}
