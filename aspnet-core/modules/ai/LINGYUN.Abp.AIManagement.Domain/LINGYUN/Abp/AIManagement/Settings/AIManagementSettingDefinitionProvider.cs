using LINGYUN.Abp.AIManagement.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.AIManagement.Settings;
public class AIManagementSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(
            new SettingDefinition(
                AIManagementSettingNames.UserMessage.MaxLatestHistoryMessagesToKeep,
                defaultValue: "5",
                displayName: L("DisplayName:MaxLatestHistoryMessagesToKeep"),
                description: L("Description:MaxLatestHistoryMessagesToKeep")));
    }

    private static ILocalizableString L(string name)
    {
        return LocalizableString.Create<AIManagementResource>(name);
    }
}
