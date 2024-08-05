using Volo.Abp.AuditLogging.Localization;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.Validation.StringValues;

namespace LINGYUN.Abp.Auditing.Features;

public class AuditingFeatureDefinitionProvider : FeatureDefinitionProvider
{
    public override void Define(IFeatureDefinitionContext context)
    {
        var auditingGroup = context.AddGroup(
            name: AuditingFeatureNames.GroupName,
            displayName: L("Features:Auditing"));

        var loggingEnableFeature = auditingGroup.AddFeature(
            name: AuditingFeatureNames.Logging.Enable,
            displayName: L("Features:Auditing"),
            description: L("Features:AuditingDesc")
            );

        loggingEnableFeature.CreateChild(
            name: AuditingFeatureNames.Logging.AuditLog,
            defaultValue: true.ToString(),
            displayName: L("Features:DisplayName:AuditLog"),
            description: L("Features:Description:AuditLog"),
            valueType: new ToggleStringValueType(new BooleanValueValidator())
            );
        loggingEnableFeature.CreateChild(
            name: AuditingFeatureNames.Logging.SecurityLog,
            defaultValue: true.ToString(),
            displayName: L("Features:DisplayName:SecurityLog"),
            description: L("Features:Description:SecurityLog"),
            valueType: new ToggleStringValueType(new BooleanValueValidator())
            );
        loggingEnableFeature.CreateChild(
            name: AuditingFeatureNames.Logging.SystemLog,
            defaultValue: true.ToString(),
            displayName: L("Features:DisplayName:SystemLog"),
            description: L("Features:Description:SystemLog"),
            valueType: new ToggleStringValueType(new BooleanValueValidator())
            );
    }

    protected LocalizableString L(string name)
    {
        return LocalizableString.Create<AuditLoggingResource>(name);
    }
}
