using LINGYUN.Abp.AIManagement.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.AIManagement.Settings;
public class AIManagementSettingDefinitionProvider : SettingDefinitionProvider
{
    private const string GroupName = "AIManagement";
    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(
            new SettingDefinition(
                AIManagementSettingNames.ChatMessage.MaxLatestHistoryMessagesToKeep,
                defaultValue: "5",
                displayName: L("DisplayName:MaxLatestHistoryMessagesToKeep"),
                description: L("Description:MaxLatestHistoryMessagesToKeep"))
            .WithProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName)
            .WithGroup(GroupName, L("Settings:AIManagement"))
            .WithParent("ChatMessage", L("Settings:AIManagement.ChatMessage"))
            .WithValueType(ValueType.Number)
        );
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AIManagementResource>(name);
    }
}
