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

        group.AddFeature(
            name: PushPlusFeatureNames.Message.Enable,
            defaultValue: "true",
            displayName: L("Features:MessageEnable"),
            description: L("Features:MessageEnableDesc"),
            valueType: new ToggleStringValueType(new BooleanValueValidator()));

        CreateWeChatChannelFeature(group);
        CreateWeWorkChannelFeature(group);
        CreateWebhookChannelFeature(group);
        CreateEmailChannelFeature(group);
        CreateSmsChannelFeature(group);
    }

    private static void CreateWeChatChannelFeature(
        FeatureGroupDefinition group)
    {
        var weChatChannel = group.AddFeature(
            name: PushPlusFeatureNames.Channel.WeChat.GroupName,
            displayName: L("Features:Channel.WeChat"),
            description: L("Features:Channel.WeChat"));
        weChatChannel.CreateChild(
            name: PushPlusFeatureNames.Channel.WeChat.Enable,
            defaultValue: "true",
            displayName: L("Features:Channel.WeChat.Enable"),
            description: L("Features:Channel.WeChat.EnableDesc"),
            valueType: new ToggleStringValueType(new BooleanValueValidator()));
        weChatChannel.CreateChild(
            name: PushPlusFeatureNames.Channel.WeChat.SendLimit,
            defaultValue: "200",
            displayName: L("Features:Channel.WeChat.SendLimit"),
            description: L("Features:Channel.WeChat.SendLimitDesc"),
            valueType: new FreeTextStringValueType(new NumericValueValidator(1, 1000)));
        weChatChannel.CreateChild(
            name: PushPlusFeatureNames.Channel.WeChat.SendLimitInterval,
            defaultValue: "1",
            displayName: L("Features:Channel.WeChat.SendLimitInterval"),
            description: L("Features:Channel.WeChat.SendLimitIntervalDesc"),
            valueType: new FreeTextStringValueType(new NumericValueValidator(1, 1)));
    }

    private static void CreateWeWorkChannelFeature(
        FeatureGroupDefinition group)
    {
        var weWorkChannel = group.AddFeature(
            name: PushPlusFeatureNames.Channel.WeWork.GroupName,
            displayName: L("Features:Channel.WeWork"),
            description: L("Features:Channel.WeWork"));
        weWorkChannel.CreateChild(
            name: PushPlusFeatureNames.Channel.WeWork.Enable,
            defaultValue: "true",
            displayName: L("Features:Channel.WeWork.Enable"),
            description: L("Features:Channel.WeWork.EnableDesc"),
            valueType: new ToggleStringValueType(new BooleanValueValidator()));
        weWorkChannel.CreateChild(
            name: PushPlusFeatureNames.Channel.WeWork.SendLimit,
            defaultValue: "200",
            displayName: L("Features:Channel.WeWork.SendLimit"),
            description: L("Features:Channel.WeWork.SendLimitDesc"),
            valueType: new FreeTextStringValueType(new NumericValueValidator(1, 10000)));
        weWorkChannel.CreateChild(
            name: PushPlusFeatureNames.Channel.WeWork.SendLimitInterval,
            defaultValue: "1",
            displayName: L("Features:Channel.WeWork.SendLimitInterval"),
            description: L("Features:Channel.WeWork.SendLimitIntervalDesc"),
            valueType: new FreeTextStringValueType(new NumericValueValidator(1, 1)));
    }

    private static void CreateWebhookChannelFeature(
        FeatureGroupDefinition group)
    {
        var webhookChannel = group.AddFeature(
            name: PushPlusFeatureNames.Channel.Webhook.GroupName,
            displayName: L("Features:Channel.Webhook"),
            description: L("Features:Channel.Webhook"));
        webhookChannel.CreateChild(
            name: PushPlusFeatureNames.Channel.Webhook.Enable,
            defaultValue: "true",
            displayName: L("Features:Channel.Webhook.Enable"),
            description: L("Features:Channel.Webhook.EnableDesc"),
            valueType: new ToggleStringValueType(new BooleanValueValidator()));
        webhookChannel.CreateChild(
            name: PushPlusFeatureNames.Channel.Webhook.SendLimit,
            defaultValue: "200",
            displayName: L("Features:Channel.Webhook.SendLimit"),
            description: L("Features:Channel.Webhook.SendLimitDesc"),
            valueType: new FreeTextStringValueType(new NumericValueValidator(1, 10000)));
        webhookChannel.CreateChild(
            name: PushPlusFeatureNames.Channel.Webhook.SendLimitInterval,
            defaultValue: "1",
            displayName: L("Features:Channel.Webhook.SendLimitInterval"),
            description: L("Features:Channel.Webhook.SendLimitIntervalDesc"),
            valueType: new FreeTextStringValueType(new NumericValueValidator(1, 1)));
    }

    private static void CreateEmailChannelFeature(
        FeatureGroupDefinition group)
    {
        var emailChannel = group.AddFeature(
            name: PushPlusFeatureNames.Channel.Email.GroupName,
            displayName: L("Features:Channel.Email"),
            description: L("Features:Channel.Email"));
        emailChannel.CreateChild(
            name: PushPlusFeatureNames.Channel.Email.Enable,
            defaultValue: "true",
            displayName: L("Features:Channel.Email.Enable"),
            description: L("Features:Channel.Email.EnableDesc"),
            valueType: new ToggleStringValueType(new BooleanValueValidator()));
        emailChannel.CreateChild(
            name: PushPlusFeatureNames.Channel.Email.SendLimit,
            defaultValue: "200",
            displayName: L("Features:Channel.Email.SendLimit"),
            description: L("Features:Channel.Email.SendLimitDesc"),
            valueType: new FreeTextStringValueType(new NumericValueValidator(1, 1000)));
        emailChannel.CreateChild(
            name: PushPlusFeatureNames.Channel.Email.SendLimitInterval,
            defaultValue: "1",
            displayName: L("Features:Channel.Email.SendLimitInterval"),
            description: L("Features:Channel.Email.SendLimitIntervalDesc"),
            valueType: new FreeTextStringValueType(new NumericValueValidator(1, 1)));
    }

    private static void CreateSmsChannelFeature(
        FeatureGroupDefinition group)
    {
        var smsChannel = group.AddFeature(
            name: PushPlusFeatureNames.Channel.Sms.GroupName,
            displayName: L("Features:Channel.Sms"),
            description: L("Features:Channel.Sms"));
        smsChannel.CreateChild(
            name: PushPlusFeatureNames.Channel.Sms.Enable,
            defaultValue: "false",
            displayName: L("Features:Channel.Sms.Enable"),
            description: L("Features:Channel.Sms.EnableDesc"),
            valueType: new ToggleStringValueType(new BooleanValueValidator()));
        smsChannel.CreateChild(
            name: PushPlusFeatureNames.Channel.Sms.SendLimit,
            defaultValue: "200",
            displayName: L("Features:Channel.Sms.SendLimit"),
            description: L("Features:Channel.Sms.SendLimitDesc"),
            valueType: new FreeTextStringValueType(new NumericValueValidator(1, 1000)));
        smsChannel.CreateChild(
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
