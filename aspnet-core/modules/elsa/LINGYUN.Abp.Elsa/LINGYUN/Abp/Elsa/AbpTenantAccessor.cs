using Elsa.Services;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Elsa;

public class AbpTenantAccessor : ITenantAccessor
{
    private readonly ICurrentTenant _currentTenant;

    public AbpTenantAccessor(
        ICurrentTenant currentTenant)
    {
        _currentTenant = currentTenant;
    }

    public Task<string?> GetTenantIdAsync(CancellationToken cancellationToken = default)
    {
        string? tenantId = null;
        if (_currentTenant.IsAvailable)
        {
            tenantId = _currentTenant.GetId().ToString();
        }
        return Task.FromResult(tenantId);
    }
}
