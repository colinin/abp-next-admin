using Volo.Abp.Features;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Validation.StringValues;

namespace LINGYUN.Abp.Identity.Features;

public class IdentityFeatureDefinitionProvider : FeatureDefinitionProvider
{
    public override void Define(IFeatureDefinitionContext context)
    {
        var identityGroup = context.AddGroup(
             name: IdentityFeatureNames.GroupName,
             displayName: L("Features:Identity"));

        identityGroup.AddFeature(
            name: IdentityFeatureNames.TwoFactor.Behaviour,
            IdentityTwoFactorBehaviour.Optional.ToString(),
            L("Features:Identity.TwoFactor"),
            L("Features:Identity.TwoFactorDesc"),
            new SelectionStringValueType
            {
                ItemSource = new StaticSelectionStringValueItemSource(
                    new LocalizableSelectionStringValueItem
                    {
                        Value = IdentityTwoFactorBehaviour.Optional.ToString(),
                        DisplayText = GetTwoFactorBehaviourLocalizableStringInfo("IdentityTwoFactorBehaviour:Optional")
                    },
                    new LocalizableSelectionStringValueItem
                    {
                        Value = IdentityTwoFactorBehaviour.Disabled.ToString(),
                        DisplayText = GetTwoFactorBehaviourLocalizableStringInfo("IdentityTwoFactorBehaviour:Disabled")
                    },
                    new LocalizableSelectionStringValueItem
                    {
                        Value = IdentityTwoFactorBehaviour.Forced.ToString(),
                        DisplayText = GetTwoFactorBehaviourLocalizableStringInfo("IdentityTwoFactorBehaviour:Forced")
                    }
                )
            });
    }

    protected static LocalizableString L(string name)
    {
        return LocalizableString.Create<IdentityResource>(name);
    }

    private static LocalizableStringInfo GetTwoFactorBehaviourLocalizableStringInfo(string key)
    {
        return new LocalizableStringInfo(LocalizationResourceNameAttribute.GetName(typeof(IdentityResource)), key);
    }
}
