using LINGYUN.Abp.ElsaNext.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.ElsaNext.Studio.Permissions;

public class AbpElsaNextStudioPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var elsa = context.AddGroup(AbpElsaNextStudioPermissionsNames.GroupName, L("Permission:ElsaNextStudio"));
        elsa.AddPermission(AbpElsaNextStudioPermissionsNames.View, L("Permission:ElsaNextStudio:View"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ElsaNextResource>(name);
    }
}
