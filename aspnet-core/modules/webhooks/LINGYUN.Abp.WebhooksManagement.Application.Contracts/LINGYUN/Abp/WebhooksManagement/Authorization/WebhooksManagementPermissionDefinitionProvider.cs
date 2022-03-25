using LINGYUN.Abp.WebhooksManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.WebhooksManagement.Authorization;

public class WebhooksManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var group = context.AddGroup(WebhooksManagementPermissions.GroupName, L("Permission:WebhooksManagement"));

        group.AddPermission(
            WebhooksManagementPermissions.ManageSettings,
            L("Permission:ManageSettings"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<WebhooksManagementResource>(name);
    }
}
