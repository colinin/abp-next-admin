namespace LINGYUN.Abp.Saas.Tenants;

public class TenantConnectionStringDto
{
    public string Name { get; set; } = default!;

    public string? Value { get; set; }
}
