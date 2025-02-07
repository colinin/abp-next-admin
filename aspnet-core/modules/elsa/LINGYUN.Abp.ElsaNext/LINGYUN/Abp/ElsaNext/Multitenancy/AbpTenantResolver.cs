using Elsa.Common.Multitenancy;
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.ElsaNext.Multitenancy;
public class AbpTenantResolver(ITenantConfigurationProvider tenantConfigurationProvider) : TenantResolverBase
{
    protected async override Task<TenantResolverResult> ResolveAsync(TenantResolverContext context)
    {
        var tenant = await tenantConfigurationProvider.GetAsync();
        if (tenant == null)
        {
            return Unresolved();
        }

        return AutoResolve(tenant.Id.ToString());
    }
}
