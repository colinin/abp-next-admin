using LINGYUN.Abp.ElsaNext.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.ElsaNext.Permissions;

public class ElsaPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        context.AddGroup(
            ElsaPermissionNames.GroupName,
            L("Permission:Elsa"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ElsaNextResource>(name);
    }
}
