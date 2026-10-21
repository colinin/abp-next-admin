using JetBrains.Annotations;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Features;

namespace LINGYUN.Abp.Identity.Features;

public static class IdentityTwoFactorBehaviourFeatureHelper
{
    public static async Task<IdentityTwoFactorBehaviour> Get([NotNull] IFeatureChecker featureChecker)
    {
        Check.NotNull(featureChecker, nameof(featureChecker));

        var value = await featureChecker.GetOrNullAsync(IdentityFeatureNames.TwoFactor.Behaviour);
        if (!value.IsNullOrWhiteSpace() && Enum.TryParse<IdentityTwoFactorBehaviour>(value, out var behaviour))
        {
            return behaviour;
        }

        return IdentityTwoFactorBehaviour.Optional;
    }
}
