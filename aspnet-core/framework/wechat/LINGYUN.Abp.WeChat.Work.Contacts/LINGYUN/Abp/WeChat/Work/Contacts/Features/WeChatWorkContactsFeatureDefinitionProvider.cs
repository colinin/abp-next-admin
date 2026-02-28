using LINGYUN.Abp.WeChat.Work.Features;
using LINGYUN.Abp.WeChat.Work.Localization;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.Validation.StringValues;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Features;
public class WeChatWorkContactsFeatureDefinitionProvider : FeatureDefinitionProvider
{
    public override void Define(IFeatureDefinitionContext context)
    {
        var weChatFeature = context.GetGroupOrNull(WeChatWorkFeatureNames.GroupName);
        if (weChatFeature == null)
        {
            return;
        }

        weChatFeature.AddFeature(
            WeChatWorkContactsFeatureNames.Enable,
            defaultValue: "false",
            displayName: L("Features:ContactsEnable"),
            description: L("Features:ContactsEnableDesc"),
            valueType: new ToggleStringValueType(new BooleanValueValidator()));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<WeChatWorkResource>(name);
    }
}
