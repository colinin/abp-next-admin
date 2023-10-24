using LINGYUN.Abp.TuiJuhe.Localization;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.Validation.StringValues;

namespace LINGYUN.Abp.TuiJuhe.Features;

public class TuiJuheFeatureDefinitionProvider : FeatureDefinitionProvider
{
    public override void Define(IFeatureDefinitionContext context)
    {
        var group = context.AddGroup(
              name: TuiJuheFeatureNames.GroupName,
              displayName: L("Features:TuiJuhe"));
        group.AddFeature(
            name: TuiJuheFeatureNames.Enable,
            defaultValue: "false",
            displayName: L("Features:TuiJuheEnable"),
            description: L("Features:TuiJuheEnableDesc"),
            valueType: new ToggleStringValueType(new BooleanValueValidator()));

        var message = group.AddFeature(
            name: TuiJuheFeatureNames.Message.GroupName,
            displayName: L("Features:Message"),
            description: L("Features:Message"));

        message.CreateChild(
            name: TuiJuheFeatureNames.Message.Enable,
            defaultValue: "false",
            displayName: L("Features:MessageEnable"),
            description: L("Features:MessageEnableDesc"),
            valueType: new ToggleStringValueType(new BooleanValueValidator()));
        message.CreateChild(
            name: TuiJuheFeatureNames.Message.SendLimit,
            defaultValue: "50",
            displayName: L("Features:Message.SendLimit"),
            description: L("Features:Message.SendLimitDesc"),
            valueType: new FreeTextStringValueType(new NumericValueValidator(1, 50)));
        message.CreateChild(
            name: TuiJuheFeatureNames.Message.SendLimitInterval,
            defaultValue: "1",
            displayName: L("Features:Message.SendLimitInterval"),
            description: L("Features:Message.SendLimitIntervalDesc"),
            valueType: new FreeTextStringValueType(new NumericValueValidator(1, 1)));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<TuiJuheResource>(name);
    }
}
