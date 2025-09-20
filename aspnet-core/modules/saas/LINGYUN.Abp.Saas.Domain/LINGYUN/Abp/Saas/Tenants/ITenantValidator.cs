using System.Threading.Tasks;

namespace LINGYUN.Abp.Saas.Tenants;
public interface ITenantValidator
{
    Task ValidateAsync(Tenant tenant);
}
