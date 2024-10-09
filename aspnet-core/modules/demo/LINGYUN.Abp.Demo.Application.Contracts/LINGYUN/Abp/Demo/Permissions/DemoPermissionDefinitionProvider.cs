using LINGYUN.Abp.Demo.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Demo.Permissions;
public class DemoPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var demoGroup = context.AddGroup(DemoPermissions.GroupName, L("Permission:Demo"));

        var booksPermission = demoGroup.AddPermission(DemoPermissions.Books.Default, L("Permission:Books"));
        booksPermission.AddChild(DemoPermissions.Books.Create, L("Permission:Books.Create"));
        booksPermission.AddChild(DemoPermissions.Books.Edit, L("Permission:Books.Edit"));
        booksPermission.AddChild(DemoPermissions.Books.Delete, L("Permission:Books.Delete"));
        booksPermission.AddChild(DemoPermissions.Books.Export, L("Permission:Books.Export"));
        booksPermission.AddChild(DemoPermissions.Books.Import, L("Permission:Books.Import"));

        var authorsPermission = demoGroup.AddPermission(
            DemoPermissions.Authors.Default, L("Permission:Authors"));
        authorsPermission.AddChild(
            DemoPermissions.Authors.Create, L("Permission:Authors.Create"));
        authorsPermission.AddChild(
            DemoPermissions.Authors.Edit, L("Permission:Authors.Edit"));
        authorsPermission.AddChild(
            DemoPermissions.Authors.Delete, L("Permission:Authors.Delete"));

    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<DemoResource>(name);
    }
}
