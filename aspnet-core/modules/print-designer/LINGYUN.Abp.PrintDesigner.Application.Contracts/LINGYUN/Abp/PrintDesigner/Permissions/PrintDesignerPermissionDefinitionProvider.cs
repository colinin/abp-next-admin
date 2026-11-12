using LINGYUN.Abp.PrintDesigner.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.PrintDesigner.Permissions;

public class PrintDesignerPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var group = context.AddGroup(PrintDesignerPermissions.GroupName, L("Permission:PrintDesigner"));

        var customElement = group.AddPermission(
            PrintDesignerPermissions.CustomElement.Default,
            L("Permission:CustomElement"));
        customElement.AddChild(
            PrintDesignerPermissions.CustomElement.Create,
            L("Permission:Create"));
        customElement.AddChild(
            PrintDesignerPermissions.CustomElement.Delete,
            L("Permission:Delete"));
        customElement.AddChild(
            PrintDesignerPermissions.CustomElement.Update,
            L("Permission:Update"));
        customElement.AddChild(
            PrintDesignerPermissions.CustomElement.Copy,
            L("Permission:Copy"));

        var template = group.AddPermission(
            PrintDesignerPermissions.Template.Default,
            L("Permission:Template"));
        template.AddChild(
            PrintDesignerPermissions.Template.Create,
            L("Permission:Create"));
        template.AddChild(
            PrintDesignerPermissions.Template.Delete,
            L("Permission:Delete"));
        template.AddChild(
            PrintDesignerPermissions.Template.Update,
            L("Permission:Update"));
        template.AddChild(
            PrintDesignerPermissions.Template.Copy,
            L("Permission:Copy"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<PrintDesignerResource>(name);
    }
}
