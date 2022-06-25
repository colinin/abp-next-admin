using LINGYUN.Abp.TextTemplating.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.TextTemplating;

public class AbpTextTemplatingPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var saasGroup = context.AddGroup(AbpTextTemplatingPermissions.GroupName, L("Permission:Saas"));

        var textTemplatePermission = saasGroup.AddPermission(AbpTextTemplatingPermissions.TextTemplate.Default, L("Permission:TextTemplates"));
        textTemplatePermission.AddChild(AbpTextTemplatingPermissions.TextTemplate.Create, L("Permission:Create"));
        textTemplatePermission.AddChild(AbpTextTemplatingPermissions.TextTemplate.Update, L("Permission:Edit"));
        textTemplatePermission.AddChild(AbpTextTemplatingPermissions.TextTemplate.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AbpTextTemplatingResource>(name);
    }
}
