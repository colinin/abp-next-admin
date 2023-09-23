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

        var wechatWorkFeature = group.AddFeature(
            name: WeChatWorkFeatureNames.Enable,
            defaultValue: "false",
            displayName: L("Features:WeChatWorkEnable"),
            description: L("Features:WeChatWorkEnableDesc"),
            valueType: new ToggleStringValueType(new BooleanValueValidator()));

        AddMessageFeature(wechatWorkFeature);
        AddAppChatFeature(wechatWorkFeature);
    }

    protected virtual void AddMessageFeature(FeatureDefinition wechatWorkFeature)
    {
        var messageEnableFeature = wechatWorkFeature.CreateChild(
            name: WeChatWorkFeatureNames.Message.Enable,
            defaultValue: "false",
            displayName: L("Features:MessageEnable"),
            description: L("Features:MessageEnableDesc"),
            valueType: new ToggleStringValueType(new BooleanValueValidator()));
        messageEnableFeature.CreateChild(
            name: WeChatWorkFeatureNames.Message.Limit,
            defaultValue: "20000",
            displayName: L("Features:Message.Limit"),
            description: L("Features:Message.LimitDesc"),
            valueType: new FreeTextStringValueType(new NumericValueValidator(1, 100000)));
        messageEnableFeature.CreateChild(
            name: WeChatWorkFeatureNames.Message.LimitInterval,
            defaultValue: "1",
            displayName: L("Features:Message.LimitInterval"),
            description: L("Features:Message.LimitIntervalDesc"),
            valueType: new FreeTextStringValueType(new NumericValueValidator(1, 1000)));
    }

    protected virtual void AddAppChatFeature(FeatureDefinition wechatWorkFeature)
    {
        var messageEnableFeature = wechatWorkFeature.CreateChild(
            name: WeChatWorkFeatureNames.AppChat.Message.Enable,
            defaultValue: "false",
            displayName: L("Features:AppChatMessageEnable"),
            description: L("Features:AppChatMessageEnableDesc"),
            valueType: new ToggleStringValueType(new BooleanValueValidator()));
        messageEnableFeature.CreateChild(
            name: WeChatWorkFeatureNames.AppChat.Message.Limit,
            defaultValue: "20000",
            displayName: L("Features:AppChatMessage.Limit"),
            description: L("Features:AppChatMessage.LimitDesc"),
            valueType: new FreeTextStringValueType(new NumericValueValidator(1, 20000)));
        messageEnableFeature.CreateChild(
            name: WeChatWorkFeatureNames.AppChat.Message.LimitInterval,
            defaultValue: "1",
            displayName: L("Features:AppChatMessage.LimitInterval"),
            description: L("Features:AppChatMessage.LimitIntervalDesc"),
            valueType: new FreeTextStringValueType(new NumericValueValidator(1, 1000)));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<WeChatWorkResource>(name);
    }
}
