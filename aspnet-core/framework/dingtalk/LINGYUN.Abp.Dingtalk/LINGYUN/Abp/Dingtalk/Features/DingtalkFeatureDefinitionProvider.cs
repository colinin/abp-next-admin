using LINGYUN.Abp.Dingtalk.Localization;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.Validation.StringValues;

namespace LINGYUN.Abp.Dingtalk.Features;

public class DingtalkFeatureDefinitionProvider : FeatureDefinitionProvider
{
    public override void Define(IFeatureDefinitionContext context)
    {
        var featureGroup = context.AddGroup(
                name: DingtalkFeatureNames.GroupName,
                displayName: L("Features:Dingtalk"));

        featureGroup.AddFeature(
            name: DingtalkFeatureNames.Enable,
            defaultValue: false.ToString(),
            displayName: L("Features:Dingtalk:IsEnabled"),
            description: L("Features:Dingtalk:IsEnabledDesc"),
            valueType: new ToggleStringValueType(new BooleanValueValidator()));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<DingtalkReousrce>(name);
    }
}
