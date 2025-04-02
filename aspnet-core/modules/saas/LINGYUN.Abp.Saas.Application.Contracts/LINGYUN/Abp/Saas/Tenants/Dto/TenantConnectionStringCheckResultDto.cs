namespace LINGYUN.Abp.Saas.Tenants;

public class TenantConnectionStringCheckResultDto
{
    public bool Connected { get; set; }
    public TenantConnectionStringCheckResultDto()
    {

    }

    public TenantConnectionStringCheckResultDto(bool connected)
    {
        Connected = connected;
    }
}
