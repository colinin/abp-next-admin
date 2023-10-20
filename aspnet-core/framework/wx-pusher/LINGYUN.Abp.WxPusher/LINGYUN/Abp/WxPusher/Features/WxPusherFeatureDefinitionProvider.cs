using LINGYUN.Abp.WxPusher.Localization;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.Validation.StringValues;

namespace LINGYUN.Abp.WxPusher.Features;

public class WxPusherFeatureDefinitionProvider : FeatureDefinitionProvider
{
    public override void Define(IFeatureDefinitionContext context)
    {
        var group = context.AddGroup(
              name: WxPusherFeatureNames.GroupName,
              displayName: L("Features:WxPusher"));
        
        var wxPusherEnableFeature = group.AddFeature(
            name: WxPusherFeatureNames.Enable,
            defaultValue: "false",
            displayName: L("Features:WxPusherEnable"),
            description: L("Features:WxPusherEnableDesc"),
            valueType: new ToggleStringValueType(new BooleanValueValidator()));

        var messageEnableFeature = wxPusherEnableFeature.CreateChild(
            name: WxPusherFeatureNames.Message.Enable,
            defaultValue: "false",
            displayName: L("Features:MessageEnable"),
            description: L("Features:MessageEnableDesc"),
            valueType: new ToggleStringValueType(new BooleanValueValidator()));
        messageEnableFeature.CreateChild(
            name: WxPusherFeatureNames.Message.SendLimit,
            defaultValue: "500",
            displayName: L("Features:Message.SendLimit"),
            description: L("Features:Message.SendLimitDesc"),
            valueType: new FreeTextStringValueType(new NumericValueValidator(1, 500)));
        messageEnableFeature.CreateChild(
            name: WxPusherFeatureNames.Message.SendLimitInterval,
            defaultValue: "1",
            displayName: L("Features:Message.SendLimitInterval"),
            description: L("Features:Message.SendLimitIntervalDesc"),
            valueType: new FreeTextStringValueType(new NumericValueValidator(1, 1)));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<WxPusherResource>(name);
    }
}
