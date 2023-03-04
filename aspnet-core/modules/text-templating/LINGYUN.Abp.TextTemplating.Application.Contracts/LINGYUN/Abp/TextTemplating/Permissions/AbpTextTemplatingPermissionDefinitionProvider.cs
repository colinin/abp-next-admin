using LINGYUN.Abp.TextTemplating.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.TextTemplating;

public class AbpTextTemplatingPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var textTemplatingGroup = context.AddGroup(AbpTextTemplatingPermissions.GroupName, L("Permission:TextTemplating"));

        var textTemplateDefinition = textTemplatingGroup.AddPermission(AbpTextTemplatingPermissions.TextTemplateDefinition.Default, L("Permission:TextTemplateDefinitions"));
        textTemplateDefinition.AddChild(AbpTextTemplatingPermissions.TextTemplateDefinition.Create, L("Permission:Create"));
        textTemplateDefinition.AddChild(AbpTextTemplatingPermissions.TextTemplateDefinition.Update, L("Permission:Edit"));
        textTemplateDefinition.AddChild(AbpTextTemplatingPermissions.TextTemplateDefinition.Delete, L("Permission:Delete"));

        var textTemplateContent = textTemplatingGroup.AddPermission(AbpTextTemplatingPermissions.TextTemplateContent.Default, L("Permission:TextTemplateContents"));
        textTemplateContent.AddChild(AbpTextTemplatingPermissions.TextTemplateContent.Update, L("Permission:Edit"));
        textTemplateContent.AddChild(AbpTextTemplatingPermissions.TextTemplateContent.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AbpTextTemplatingResource>(name);
    }
}
