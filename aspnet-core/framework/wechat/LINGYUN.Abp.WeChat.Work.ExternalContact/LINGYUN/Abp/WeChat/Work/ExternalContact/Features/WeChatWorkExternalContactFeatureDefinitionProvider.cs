using LINGYUN.Abp.WeChat.Work.Features;
using LINGYUN.Abp.WeChat.Work.Localization;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.Validation.StringValues;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Features;
public class WeChatWorkExternalContactFeatureDefinitionProvider : FeatureDefinitionProvider
{
    public override void Define(IFeatureDefinitionContext context)
    {
        var weChatFeature = context.GetGroupOrNull(WeChatWorkFeatureNames.GroupName);
        if (weChatFeature == null)
        {
            return;
        }
        weChatFeature.AddFeature(
            WeChatWorkExternalContactFeatureNames.Enable,
            defaultValue: "false",
            displayName: L("Features:ExternalContactEnable"),
            description: L("Features:ExternalContactEnableDesc"),
            valueType: new ToggleStringValueType(new BooleanValueValidator()));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<WeChatWorkResource>(name);
    }
}
