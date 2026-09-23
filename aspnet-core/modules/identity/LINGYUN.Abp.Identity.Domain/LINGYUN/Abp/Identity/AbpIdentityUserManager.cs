using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;
using Volo.Abp.Settings;
using Volo.Abp.Threading;
using Volo.Abp.Users;

namespace LINGYUN.Abp.Identity;

[Dependency(ReplaceServices = true)]
[ExposeServices(
    typeof(IdentityUserManager),
    typeof(AbpIdentityUserManager),
    typeof(UserManager<IdentityUser>))]
public class AbpIdentityUserManager : IdentityUserManager
{
    public AbpIdentityUserManager(
        IdentityUserStore store,
        Volo.Abp.Identity.IIdentityRoleRepository roleRepository,
        Volo.Abp.Identity.IIdentityUserRepository userRepository,
        IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<IdentityUser> passwordHasher,
        IEnumerable<IUserValidator<IdentityUser>> userValidators,
        IEnumerable<IPasswordValidator<IdentityUser>> passwordValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        IServiceProvider services,
        ILogger<IdentityUserManager> logger,
        ICancellationTokenProvider cancellationTokenProvider,
        Volo.Abp.Identity.IOrganizationUnitRepository organizationUnitRepository,
        ISettingProvider settingProvider,
        IDistributedEventBus distributedEventBus,
        IIdentityLinkUserRepository identityLinkUserRepository,
        IDistributedCache<AbpDynamicClaimCacheItem> dynamicClaimCache,
        IOptions<AbpMultiTenancyOptions> multiTenancyOptions,
        ICurrentTenant currentTenant,
        IDataFilter dataFilter)
      : base(
        store,
        roleRepository,
        userRepository,
        optionsAccessor,
        passwordHasher,
        userValidators,
        passwordValidators,
        keyNormalizer,
        errors,
        services,
        logger,
        cancellationTokenProvider,
        organizationUnitRepository,
        settingProvider,
        distributedEventBus,
        identityLinkUserRepository,
        dynamicClaimCache,
        multiTenancyOptions,
        currentTenant,
        dataFilter)
    {
    }

    public async override Task<IdentityResult> ResetPasswordAsync(IdentityUser user, string token, string newPassword)
    {
        var result = await base.ResetPasswordAsync(user, token, newPassword);

        result.CheckErrors();

        var currentUser = ServiceProvider.GetService<ICurrentUser>();

        await DistributedEventBus.PublishAsync(new IdentityUserSessionPasswordChangedEto
        {
            Id = user.Id,
            TenantId = user.TenantId,
            Email = user.Email,
            SessionId = currentUser?.FindSessionId(),
        });

        return result;
    }

    public async override Task<IdentityResult> ChangePasswordAsync(IdentityUser user, string currentPassword, string newPassword)
    {
        var result = await base.ChangePasswordAsync(user, currentPassword, newPassword);

        result.CheckErrors();

        var currentUser = ServiceProvider.GetService<ICurrentUser>();

        await DistributedEventBus.PublishAsync(new IdentityUserSessionPasswordChangedEto
        {
            Id = user.Id,
            TenantId = user.TenantId,
            Email = user.Email,
            SessionId = currentUser?.FindSessionId(),
        });

        return result;
    }
}
