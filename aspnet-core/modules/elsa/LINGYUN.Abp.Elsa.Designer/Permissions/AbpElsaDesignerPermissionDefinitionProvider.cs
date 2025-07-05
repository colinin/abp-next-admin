using LINGYUN.Abp.Elsa.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Elsa.Designer.Permissions;

public class AbpElsaDesignerPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var elsa = context.AddGroup(AbpElsaDesignerPermissionsNames.GroupName, L("Permission:ElsaDesigner"));
        elsa.AddPermission(AbpElsaDesignerPermissionsNames.View, L("Permission:ElsaDesigner:View"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ElsaResource>(name);
    }
}
