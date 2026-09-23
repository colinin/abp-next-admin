using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AspNetCore.Mvc.UI.MultiTenancy;

[Dependency(TryRegister = true)]
public class NullAvailableTenantPickerStore : IAvailableTenantPickerStore, ISingletonDependency
{
    private readonly static List<NameValue<string>> _emptyAvailableTenants = new();
    public Task<List<NameValue<string>>> GetAvailableTenantsAsync()
    {
        return Task.FromResult(_emptyAvailableTenants);
    }
}
