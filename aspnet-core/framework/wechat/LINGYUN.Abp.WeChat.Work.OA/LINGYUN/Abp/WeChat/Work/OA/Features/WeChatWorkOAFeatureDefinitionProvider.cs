using LINGYUN.Abp.WeChat.Work.Features;
using LINGYUN.Abp.WeChat.Work.Localization;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.Validation.StringValues;

namespace LINGYUN.Abp.WeChat.Work.OA.Features;

public class WeChatWorkOAFeatureDefinitionProvider : FeatureDefinitionProvider
{
    public override void Define(IFeatureDefinitionContext context)
    {
        var weChatFeature = context.GetGroupOrNull(WeChatWorkFeatureNames.GroupName);
        if (weChatFeature == null)
        {
            return;
        }
        weChatFeature.AddFeature(
            WeChatWorkOAFeatureNames.Enable,
            defaultValue: "false",
            displayName: L("Features:OAEnable"),
            description: L("Features:OAEnableDesc"),
            valueType: new ToggleStringValueType(new BooleanValueValidator()));
    }

    private static ILocalizableString L(string name)
    {
        return LocalizableString.Create<WeChatWorkResource>(name);
    }
}
