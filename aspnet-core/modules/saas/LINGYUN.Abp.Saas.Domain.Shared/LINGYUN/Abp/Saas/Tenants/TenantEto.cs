using System;

namespace LINGYUN.Abp.Saas.Tenants;

[Serializable]
public class TenantEto
{
    public Guid Id { get; set; }

    public string Name { get; set; }
}
