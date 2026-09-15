using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.Identity;

[Dependency(ReplaceServices = true)]
[ExposeServices(
    typeof(IdentityUserStore),
    typeof(AbpIdentityUserStore),
    typeof(IUserStore<IdentityUser>),
    typeof(IUserTwoFactorStore<IdentityUser>))]
public class AbpIdentityUserStore : IdentityUserStore
{
    protected IdentityTwoFactorManager IdentityTwoFactorManager { get; }
    public AbpIdentityUserStore(
        IdentityTwoFactorManager identityTwoFactorManager,
        Volo.Abp.Identity.IIdentityUserRepository userRepository, 
        Volo.Abp.Identity.IIdentityRoleRepository roleRepository, 
        IGuidGenerator guidGenerator, 
        ILogger<IdentityRoleStore> logger, 
        ILookupNormalizer lookupNormalizer,
        IdentityErrorDescriber? describer = null) 
        : base(userRepository, roleRepository, guidGenerator, logger, lookupNormalizer, describer)
    {
        IdentityTwoFactorManager = identityTwoFactorManager;
    }

    public async override Task<bool> GetTwoFactorEnabledAsync(IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        return await IdentityTwoFactorManager.GetTwoFactorEnabledAsync(user);
    }
}
