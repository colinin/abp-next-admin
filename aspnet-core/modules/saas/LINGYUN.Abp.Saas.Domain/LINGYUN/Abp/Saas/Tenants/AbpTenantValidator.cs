using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Saas.Tenants;
public class AbpTenantValidator : ITenantValidator, ITransientDependency
{
    protected ITenantRepository TenantRepository { get; }

    public AbpTenantValidator(ITenantRepository tenantRepository)
    {
        TenantRepository = tenantRepository;
    }

    public virtual async Task ValidateAsync(Tenant tenant)
    {
        Check.NotNullOrWhiteSpace(tenant.Name, nameof(tenant.Name));
        Check.NotNullOrWhiteSpace(tenant.NormalizedName, nameof(tenant.NormalizedName));

        var owner = await TenantRepository.FindByNameAsync(tenant.NormalizedName);
        if (owner != null && owner.Id != tenant.Id)
        {
            throw new BusinessException(AbpSaasErrorCodes.DuplicateTenantName)
                 .WithData("Name", tenant.NormalizedName);
        }
    }
}
