using LINGYUN.Abp.LocalizationManagement.Localization;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.Validation.StringValues;

namespace LINGYUN.Abp.LocalizationManagement.Features;

public class LocalizationManagementFeatureDefinitionProvider : FeatureDefinitionProvider
{
    public override void Define(IFeatureDefinitionContext context)
    {
        var group = context.AddGroup(LocalizationManagementFeatures.GroupName,
            L("Feature:LocalizationManagement"));
        group.AddFeature(LocalizationManagementFeatures.Enable,
            "true",
            L("Feature:LocalizationManagementEnable"),
            L("Feature:LocalizationManagementEnableDesc"),
            new ToggleStringValueType());
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<LocalizationManagementResource>(name);
    }
}
