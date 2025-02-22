using Elsa.Common.Multitenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.ElsaNext.Multitenancy;
public class AbpTenantsProvider : ITenantsProvider
{
    private readonly ITenantStore _tenantStore;
    public AbpTenantsProvider(ITenantStore tenantStore)
    {
        _tenantStore = tenantStore;
    }

    public async virtual Task<Tenant?> FindAsync(TenantFilter filter, CancellationToken cancellationToken = default)
    {
        if (Guid.TryParse(filter.Id, out var tenantId))
        {
            var tenant = await _tenantStore.FindAsync(tenantId);
            return tenant != null ? new Tenant
            {
                Id = tenant.Id.ToString(),
                Name = tenant.Name,
            } : null;
        }
        return null;
    }

    public async virtual Task<IEnumerable<Tenant>> ListAsync(CancellationToken cancellationToken = default)
    {
        var tenants = await _tenantStore.GetListAsync();

        return tenants.Select(tenant => new Tenant
        {
            Id = tenant.Id.ToString(),
            Name = tenant.Name
        });
    }
}
