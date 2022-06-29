using Elsa.Services;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Workflow.Elsa;

public class AbpTenantAccessor : ITenantAccessor
{
    private readonly ICurrentTenant _currentTenant;

    public AbpTenantAccessor(
        ICurrentTenant currentTenant)
    {
        _currentTenant = currentTenant;
    }

    public Task<string> GetTenantIdAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTenant.IsAvailable)
        {
            return Task.FromResult(_currentTenant.GetId().ToString());
        }
        return Task.FromResult("");
    }
}
