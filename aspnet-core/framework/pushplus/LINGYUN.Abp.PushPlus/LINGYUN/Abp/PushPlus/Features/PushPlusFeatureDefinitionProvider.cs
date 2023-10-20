using LINGYUN.Abp.PushPlus.Localization;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.Validation.StringValues;

namespace LINGYUN.Abp.PushPlus.Features;
public class PushPlusFeatureDefinitionProvider : FeatureDefinitionProvider
{
    public override void Define(IFeatureDefinitionContext context)
    {
        var group = context.AddGroup(
            name: PushPlusFeatureNames.GroupName,
            displayName: L("Features:PushPlus"));

       var pushPlusMessageEnableFeature = group.AddFeature(
            name: PushPlusFeatureNames.Message.Enable,
            defaultValue: "false",
            displayName: L("Features:MessageEnable"),
            description: L("Features:MessageEnableDesc"),
            valueType: new ToggleStringValueType(new BooleanValueValidator()));

        CreateWeChatChannelFeature(pushPlusMessageEnableFeature);
        CreateWeWorkChannelFeature(pushPlusMessageEnableFeature);
        CreateWebhookChannelFeature(pushPlusMessageEnableFeature);
        CreateEmailChannelFeature(pushPlusMessageEnableFeature);
        CreateSmsChannelFeature(pushPlusMessageEnableFeature);
    }

    private static void CreateWeChatChannelFeature(
        FeatureDefinition pushPlusMessageEnableFeature)
    {
        var weChatChannelEnableFeature = pushPlusMessageEnableFeature.CreateChild(
            name: PushPlusFeatureNames.Channel.WeChat.Enable,
            defaultValue: "false",
            displayName: L("Features:Channel.WeChat.Enable"),
            description: L("Features:Channel.WeChat.EnableDesc"),
            valueType: new ToggleStringValueType(new BooleanValueValidator()));
        weChatChannelEnableFeature.CreateChild(
            name: PushPlusFeatureNames.Channel.WeChat.SendLimit,
            defaultValue: "200",
            displayName: L("Features:Channel.WeChat.SendLimit"),
            description: L("Features:Channel.WeChat.SendLimitDesc"),
            valueType: new FreeTextStringValueType(new NumericValueValidator(1, 1000)));
        weChatChannelEnableFeature.CreateChild(
            name: PushPlusFeatureNames.Channel.WeChat.SendLimitInterval,
            defaultValue: "1",
            displayName: L("Features:Channel.WeChat.SendLimitInterval"),
            description: L("Features:Channel.WeChat.SendLimitIntervalDesc"),
            valueType: new FreeTextStringValueType(new NumericValueValidator(1, 1)));
    }

    private static void CreateWeWorkChannelFeature(
        FeatureDefinition pushPlusMessageEnableFeature)
    {
        var weWorkChannelEnableFeature = pushPlusMessageEnableFeature.CreateChild(
            name: PushPlusFeatureNames.Channel.WeWork.Enable,
            defaultValue: "false",
            displayName: L("Features:Channel.WeWork.Enable"),
            description: L("Features:Channel.WeWork.EnableDesc"),
            valueType: new ToggleStringValueType(new BooleanValueValidator()));
        weWorkChannelEnableFeature.CreateChild(
            name: PushPlusFeatureNames.Channel.WeWork.SendLimit,
            defaultValue: "200",
            displayName: L("Features:Channel.WeWork.SendLimit"),
            description: L("Features:Channel.WeWork.SendLimitDesc"),
            valueType: new FreeTextStringValueType(new NumericValueValidator(1, 10000)));
        weWorkChannelEnableFeature.CreateChild(
            name: PushPlusFeatureNames.Channel.WeWork.SendLimitInterval,
            defaultValue: "1",
            displayName: L("Features:Channel.WeWork.SendLimitInterval"),
            description: L("Features:Channel.WeWork.SendLimitIntervalDesc"),
            valueType: new FreeTextStringValueType(new NumericValueValidator(1, 1)));
    }

    private static void CreateWebhookChannelFeature(
        FeatureDefinition pushPlusMessageEnableFeature)
    {
        var webhookChannelEnableFeatuer = pushPlusMessageEnableFeature.CreateChild(
            name: PushPlusFeatureNames.Channel.Webhook.Enable,
            defaultValue: "false",
            displayName: L("Features:Channel.Webhook.Enable"),
            description: L("Features:Channel.Webhook.EnableDesc"),
            valueType: new ToggleStringValueType(new BooleanValueValidator()));
        webhookChannelEnableFeatuer.CreateChild(
            name: PushPlusFeatureNames.Channel.Webhook.SendLimit,
            defaultValue: "200",
            displayName: L("Features:Channel.Webhook.SendLimit"),
            description: L("Features:Channel.Webhook.SendLimitDesc"),
            valueType: new FreeTextStringValueType(new NumericValueValidator(1, 10000)));
        webhookChannelEnableFeatuer.CreateChild(
            name: PushPlusFeatureNames.Channel.Webhook.SendLimitInterval,
            defaultValue: "1",
            displayName: L("Features:Channel.Webhook.SendLimitInterval"),
            description: L("Features:Channel.Webhook.SendLimitIntervalDesc"),
            valueType: new FreeTextStringValueType(new NumericValueValidator(1, 1)));
    }

    private static void CreateEmailChannelFeature(
        FeatureDefinition pushPlusMessageEnableFeature)
    {
        var emailChannelEnableFeature = pushPlusMessageEnableFeature.CreateChild(
            name: PushPlusFeatureNames.Channel.Email.Enable,
            defaultValue: "false",
            displayName: L("Features:Channel.Email.Enable"),
            description: L("Features:Channel.Email.EnableDesc"),
            valueType: new ToggleStringValueType(new BooleanValueValidator()));
        emailChannelEnableFeature.CreateChild(
            name: PushPlusFeatureNames.Channel.Email.SendLimit,
            defaultValue: "200",
            displayName: L("Features:Channel.Email.SendLimit"),
            description: L("Features:Channel.Email.SendLimitDesc"),
            valueType: new FreeTextStringValueType(new NumericValueValidator(1, 1000)));
        emailChannelEnableFeature.CreateChild(
            name: PushPlusFeatureNames.Channel.Email.SendLimitInterval,
            defaultValue: "1",
            displayName: L("Features:Channel.Email.SendLimitInterval"),
            description: L("Features:Channel.Email.SendLimitIntervalDesc"),
            valueType: new FreeTextStringValueType(new NumericValueValidator(1, 1)));
    }

    private static void CreateSmsChannelFeature(
        FeatureDefinition pushPlusMessageEnableFeature)
    {
        var smsChannelEnableFeature = pushPlusMessageEnableFeature.CreateChild(
            name: PushPlusFeatureNames.Channel.Sms.Enable,
            defaultValue: "false",
            displayName: L("Features:Channel.Sms.Enable"),
            description: L("Features:Channel.Sms.EnableDesc"),
            valueType: new ToggleStringValueType(new BooleanValueValidator()));
        smsChannelEnableFeature.CreateChild(
            name: PushPlusFeatureNames.Channel.Sms.SendLimit,
            defaultValue: "200",
            displayName: L("Features:Channel.Sms.SendLimit"),
            description: L("Features:Channel.Sms.SendLimitDesc"),
            valueType: new FreeTextStringValueType(new NumericValueValidator(1, 1000)));
        smsChannelEnableFeature.CreateChild(
            name: PushPlusFeatureNames.Channel.Sms.SendLimitInterval,
            defaultValue: "1",
            displayName: L("Features:Channel.Sms.SendLimitInterval"),
            description: L("Features:Channel.Sms.SendLimitIntervalDesc"),
            valueType: new FreeTextStringValueType(new NumericValueValidator(1, 1)));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<PushPlusResource>(name);
    }
}
