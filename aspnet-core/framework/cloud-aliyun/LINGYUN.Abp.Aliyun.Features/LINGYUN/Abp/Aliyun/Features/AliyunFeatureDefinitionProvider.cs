using LINGYUN.Abp.Aliyun.Localization;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.Validation.StringValues;

namespace LINGYUN.Abp.Aliyun.Features;
public class AliyunFeatureDefinitionProvider : FeatureDefinitionProvider
{
    public override void Define(IFeatureDefinitionContext context)
    {
        var featureGroup = context.AddGroup(
                name: AliyunFeatureNames.GroupName,
                displayName: L("Features:AlibabaCloud"));

        featureGroup.AddFeature(
            name: AliyunFeatureNames.IsEnabled,
            defaultValue: false.ToString(),
            displayName: L("Features:AlibabaCloud:IsEnabled"),
            description: L("Features:AlibabaCloud:IsEnabledDesc"),
            valueType: new ToggleStringValueType(new BooleanValueValidator()));
    }

    private static ILocalizableString L(string name)
    {
        return LocalizableString.Create<AliyunResource>(name);
    }
}
