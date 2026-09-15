using LINGYUN.Abp.Identity.Features;
using LINGYUN.Abp.Identity.Settings;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.Features;
using Volo.Abp.Identity;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Identity;

public class IdentityTwoFactorManager : DomainService
{
    protected IFeatureChecker FeatureChecker { get; }

    protected ISettingProvider SettingProvider { get; }

    public IdentityTwoFactorManager(IFeatureChecker featureChecker, ISettingProvider settingProvider)
    {
        FeatureChecker = featureChecker;
        SettingProvider = settingProvider;
    }

    public virtual async Task<bool> IsOptionalAsync()
    {
        var feature = await IdentityTwoFactorBehaviourFeatureHelper.Get(FeatureChecker);
        if (feature == IdentityTwoFactorBehaviour.Optional)
        {
            var setting = await IdentityTwoFactorBehaviourSettingHelper.Get(SettingProvider);
            if (setting == IdentityTwoFactorBehaviour.Optional)
            {
                return true;
            }
        }

        return false;
    }

    public virtual async Task<bool> IsForcedEnableAsync()
    {
        var feature = await IdentityTwoFactorBehaviourFeatureHelper.Get(FeatureChecker);
        if (feature == IdentityTwoFactorBehaviour.Forced)
        {
            return true;
        }

        var setting = await IdentityTwoFactorBehaviourSettingHelper.Get(SettingProvider);
        if (setting == IdentityTwoFactorBehaviour.Forced)
        {
            return true;
        }

        return false;
    }

    public virtual async Task<bool> IsForcedDisableAsync()
    {
        var feature = await IdentityTwoFactorBehaviourFeatureHelper.Get(FeatureChecker);
        if (feature == IdentityTwoFactorBehaviour.Disabled)
        {
            return true;
        }

        var setting = await IdentityTwoFactorBehaviourSettingHelper.Get(SettingProvider);
        if (setting == IdentityTwoFactorBehaviour.Disabled)
        {
            return true;
        }
        return false;
    }

    public virtual async Task<bool> GetTwoFactorEnabledAsync(IdentityUser user)
    {
        if (await IsForcedDisableAsync())
        {
            return false;
        }
        if (await IsForcedEnableAsync())
        {
            return true;
        }

        return user.TwoFactorEnabled;
    }
}
