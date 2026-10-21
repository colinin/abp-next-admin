using PackageName.CompanyName.ProjectName.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace PackageName.CompanyName.ProjectName.Settings;

public class ProjectNameSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        // Example define:
        //context.Add(
        //    new SettingDefinition(
        //        ProjectNameSettings.ExampleSetting,
        //        displayName: L("Settings:ExampleSetting"),
        //        description: L("Settings:ExampleSettingDesc"),
        //        isVisibleToClients: false,
        //        isEncrypted: true)
        //    .WithGroup(ProjectNameSettings.GroupName, L("Settings:ProjectName"))
        //    .WithParent("ExampleSetting", L("Settings:Parent"), order: 1)
        //    .WithValueType(ValueType.String));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ProjectNameResource>(name);
    }
}
