using LINGYUN.Abp.Authorization.OrganizationUnits;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.ChangeTracking;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;

namespace LINGYUN.Abp.Identity.OrganizaztionUnits;

[Dependency(ServiceLifetime.Transient, ReplaceServices = true)]
[ExposeServices(
    typeof(IdentityDynamicClaimsPrincipalContributorCache),
    typeof(OrganizaztionUnitDynamicClaimsPrincipalContributorCache))]
public class OrganizaztionUnitDynamicClaimsPrincipalContributorCache : IdentityDynamicClaimsPrincipalContributorCache
{
    protected IIdentityUserRepository IdentityUserRepository { get; }
    protected IIdentityRoleRepository IdentityRoleRepository { get; }

    public OrganizaztionUnitDynamicClaimsPrincipalContributorCache(
        IDistributedCache<AbpDynamicClaimCacheItem> dynamicClaimCache, 
        ICurrentTenant currentTenant, 
        IdentityUserManager userManager,
        IIdentityUserRepository identityUserRepository,
        IIdentityRoleRepository identityRoleRepository,
        IUserClaimsPrincipalFactory<IdentityUser> userClaimsPrincipalFactory,
        IOptions<AbpClaimsPrincipalFactoryOptions> abpClaimsPrincipalFactoryOptions, 
        IOptions<IdentityDynamicClaimsPrincipalContributorCacheOptions> cacheOptions) 
        : base(dynamicClaimCache, currentTenant, userManager, userClaimsPrincipalFactory, abpClaimsPrincipalFactoryOptions, cacheOptions)
    {
        IdentityUserRepository = identityUserRepository;
        IdentityRoleRepository = identityRoleRepository;
    }

    [DisableEntityChangeTracking]
    public async override Task<AbpDynamicClaimCacheItem> GetAsync(Guid userId, Guid? tenantId = null)
    {
        var cacheItems = await base.GetAsync(userId, tenantId);
        if (cacheItems.Claims.Any(x => x.Type == AbpOrganizationUnitClaimTypes.OrganizationUnit && x.Value.IsNullOrWhiteSpace()))
        {
            cacheItems.Claims.RemoveAll(x => x.Type == AbpOrganizationUnitClaimTypes.OrganizationUnit);

            var userOus = await IdentityUserRepository.GetOrganizationUnitsAsync(id: userId);

            foreach (var userOu in userOus)
            {
                if (!cacheItems.Claims.Any(x => x.Type == AbpOrganizationUnitClaimTypes.OrganizationUnit && x.Value == userOu.Code))
                {
                    cacheItems.Claims.Add(new AbpDynamicClaim(AbpOrganizationUnitClaimTypes.OrganizationUnit, userOu.Code));
                }
            }

            var userRoles = cacheItems.Claims
                .FindAll(x => x.Type == AbpClaimTypes.Role)
                .Select(x => x.Value)
                .Distinct();

            var roleOus = await IdentityRoleRepository.GetOrganizationUnitsAsync(userRoles);
            foreach (var roleOu in roleOus)
            {
                if (!cacheItems.Claims.Any(x => x.Type == AbpOrganizationUnitClaimTypes.OrganizationUnit && x.Value == roleOu.Code))
                {
                    cacheItems.Claims.Add(new AbpDynamicClaim(AbpOrganizationUnitClaimTypes.OrganizationUnit, roleOu.Code));
                }
            }

            await DynamicClaimCache.SetAsync(
                AbpDynamicClaimCacheItem.CalculateCacheKey(userId, tenantId), 
                cacheItems, 
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = CacheOptions.Value.CacheAbsoluteExpiration
                });
        }

        return cacheItems;
    }
}
