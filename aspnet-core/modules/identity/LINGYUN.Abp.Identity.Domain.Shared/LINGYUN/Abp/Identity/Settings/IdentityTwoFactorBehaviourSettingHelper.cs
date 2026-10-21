using JetBrains.Annotations;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Identity.Settings;

public static class IdentityTwoFactorBehaviourSettingHelper
{
    public static async Task<IdentityTwoFactorBehaviour> Get([NotNull] ISettingProvider settingProvider)
    {
        Check.NotNull(settingProvider, nameof(settingProvider));

        var value = await settingProvider.GetOrNullAsync(IdentitySettingNames.Security.TwoFactorBehaviour);
        if (!value.IsNullOrWhiteSpace() && Enum.TryParse<IdentityTwoFactorBehaviour>(value, out var behaviour))
        {
            return behaviour;
        }

        return IdentityTwoFactorBehaviour.Optional;
    }
}
