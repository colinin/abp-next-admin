using LINGYUN.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using LINGYUN.Platform.Portal;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.MicroService.AuthServer.MultiTenancy;

[Dependency(ReplaceServices = true)]
public class PaltformAvailableTenantPickerStore : IAvailableTenantPickerStore, ITransientDependency
{
    private readonly IEnterpriseRepository _enterpriseRepository;
    public PaltformAvailableTenantPickerStore(IEnterpriseRepository enterpriseRepository)
    {
        _enterpriseRepository = enterpriseRepository;
    }

    public async virtual Task<List<NameValue<string>>> GetAvailableTenantsAsync()
    {
        var enterprises = await _enterpriseRepository.GetEnterprisesInTenantListAsync(maxResultCount: 25);

        return enterprises
            .Select(enterprise => new NameValue<string>(enterprise.Name, enterprise.TenantId?.ToString()))
            .ToList();
    }
}
