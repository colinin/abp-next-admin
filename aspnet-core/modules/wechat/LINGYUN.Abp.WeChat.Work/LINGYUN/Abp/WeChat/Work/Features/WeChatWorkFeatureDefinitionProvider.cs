using LINGYUN.Abp.WeChat.Work.Localization;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.Validation.StringValues;

namespace LINGYUN.Abp.WeChat.Work.Features;
public class WeChatWorkFeatureDefinitionProvider : FeatureDefinitionProvider
{
    public override void Define(IFeatureDefinitionContext context)
    {
        var group = context.AddGroup(
              name: WeChatWorkFeatureNames.GroupName,
              displayName: L("Features:WeChatWork"));

        var weChatWorkEnableFeature = group.AddFeature(
            name: WeChatWorkFeatureNames.Enable,
            defaultValue: "false",
            displayName: L("Features:WeChatWorkEnable"),
            description: L("Features:WeChatWorkEnableDesc"),
            valueType: new ToggleStringValueType(new BooleanValueValidator()));

        var messageEnableFeature = weChatWorkEnableFeature.CreateChild(
            name: WeChatWorkFeatureNames.Message.Enable,
            defaultValue: "false",
            displayName: L("Features:MessageEnable"),
            description: L("Features:MessageEnableDesc"),
            valueType: new ToggleStringValueType(new BooleanValueValidator()));
        messageEnableFeature.CreateChild(
            name: WeChatWorkFeatureNames.Message.SendLimit,
            defaultValue: "20000",
            displayName: L("Features:Message.SendLimit"),
            description: L("Features:Message.SendLimitDesc"),
            valueType: new FreeTextStringValueType(new NumericValueValidator(1, 100000)));
        messageEnableFeature.CreateChild(
            name: WeChatWorkFeatureNames.Message.SendLimitInterval,
            defaultValue: "1",
            displayName: L("Features:Message.SendLimitInterval"),
            description: L("Features:Message.SendLimitIntervalDesc"),
            valueType: new FreeTextStringValueType(new NumericValueValidator(1, 1)));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<WeChatWorkResource>(name);
    }
}
