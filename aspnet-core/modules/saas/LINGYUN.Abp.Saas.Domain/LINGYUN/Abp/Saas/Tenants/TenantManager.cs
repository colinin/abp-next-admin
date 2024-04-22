using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Saas.Tenants;

public class TenantManager : DomainService, ITenantManager
{
    protected ITenantRepository TenantRepository { get; }
    protected ITenantNormalizer TenantNormalizer { get; }

    public TenantManager(
        ITenantRepository tenantRepository,
        ITenantNormalizer tenantNormalizer)
    {
        TenantRepository = tenantRepository;
        TenantNormalizer = tenantNormalizer;
    }

    public virtual async Task<Tenant> CreateAsync(string name)
    {
        Check.NotNull(name, nameof(name));

        var normalizedName = TenantNormalizer.NormalizeName(name);
        await ValidateNameAsync(normalizedName);
        return new Tenant(GuidGenerator.Create(), name, normalizedName);
    }

    public virtual async Task ChangeNameAsync(Tenant tenant, string name)
    {
        Check.NotNull(tenant, nameof(tenant));
        Check.NotNull(name, nameof(name));

        var normalizedName = TenantNormalizer.NormalizeName(name);

        await ValidateNameAsync(normalizedName, tenant.Id);
        tenant.SetName(name);
        tenant.SetNormalizedName(normalizedName);

    }

    protected virtual async Task ValidateNameAsync(string normalizeName, Guid? expectedId = null)
    {
        var tenant = await TenantRepository.FindByNameAsync(normalizeName);
        if (tenant != null && tenant.Id != expectedId)
        {
            throw new BusinessException(AbpSaasErrorCodes.DuplicateTenantName)
                .WithData("Name", normalizeName);
        }
    }
}
