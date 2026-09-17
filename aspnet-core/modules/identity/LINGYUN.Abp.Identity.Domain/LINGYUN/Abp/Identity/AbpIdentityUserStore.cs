using LINGYUN.Abp.Identity.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.Roles;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Identity;

[Dependency(ReplaceServices = true)]
[ExposeServices(
    typeof(IdentityUserStore),
    typeof(AbpIdentityUserStore),
    typeof(IUserStore<IdentityUser>),
    typeof(IUserTwoFactorStore<IdentityUser>))]
public class AbpIdentityUserStore : IdentityUserStore
{
    protected ISettingProvider SettingProvider { get; }
    protected IdentityTwoFactorManager IdentityTwoFactorManager { get; }
    public AbpIdentityUserStore(
        ISettingProvider settingProvider,
        IdentityTwoFactorManager identityTwoFactorManager,
        Volo.Abp.Identity.IIdentityUserRepository userRepository, 
        Volo.Abp.Identity.IIdentityRoleRepository roleRepository, 
        IGuidGenerator guidGenerator, 
        ILogger<IdentityRoleStore> logger, 
        ILookupNormalizer lookupNormalizer,
        IdentityErrorDescriber? describer = null) 
        : base(userRepository, roleRepository, guidGenerator, logger, lookupNormalizer, describer)
    {
        SettingProvider = settingProvider;
        IdentityTwoFactorManager = identityTwoFactorManager;
    }

    public async override Task<IdentityResult> CreateAsync(IdentityUser user, CancellationToken cancellationToken = default)
    {
        if (await IdentityTwoFactorManager.IsForcedEnableAsync())
        {
            await SetTwoFactorEnabledAsync(user, true, cancellationToken);
        }

        if (!user.TwoFactorEnabled &&
            await IsInRoleAsync(user, AbpRoleConsts.AdminRoleName, cancellationToken) &&
            await SettingProvider.IsTrueAsync(IdentitySettingNames.Security.AdminRoleTwoFactorForceEnabled))
        {
            await SetTwoFactorEnabledAsync(user, true, cancellationToken);
        }

        return await base.CreateAsync(user, cancellationToken);
    }

    public async override Task<bool> GetTwoFactorEnabledAsync(IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        var isInAdminRole = await IsInRoleAsync(user, AbpRoleConsts.AdminRoleName, cancellationToken);

        return await IdentityTwoFactorManager.GetTwoFactorEnabledAsync(user, isInAdminRole);
    }
}
