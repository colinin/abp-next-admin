using LINGYUN.Platform.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace LINGYUN.Platform.Permissions
{
    public class PlatformPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var platform = context.AddGroup(PlatformPermissions.GroupName, L("Permission:Platform"));

            var appVersion = platform.AddPermission(PlatformPermissions.AppVersion.Default, L("Permission:AppVersion"));
            appVersion.AddChild(PlatformPermissions.AppVersion.Create, L("Permission:CreateVersion"));
            appVersion.AddChild(PlatformPermissions.AppVersion.Delete, L("Permission:DeleteVersion"));

            var versionFile = appVersion.AddChild(PlatformPermissions.AppVersion.FileManager.Default, L("Permission:FileManager"));
            versionFile.AddChild(PlatformPermissions.AppVersion.FileManager.Create, L("Permission:AppendFile"));
            versionFile.AddChild(PlatformPermissions.AppVersion.FileManager.Delete, L("Permission:DeleteFile"));
            versionFile.AddChild(PlatformPermissions.AppVersion.FileManager.Download, L("Permission:DownloadFile"));

            var dataDictionary = platform.AddPermission(PlatformPermissions.DataDictionary.Default, L("Permission:DataDictionary"));
            dataDictionary.AddChild(PlatformPermissions.DataDictionary.Create, L("Permission:Create"));
            dataDictionary.AddChild(PlatformPermissions.DataDictionary.Update, L("Permission:Update"));
            dataDictionary.AddChild(PlatformPermissions.DataDictionary.Move, L("Permission:Move"));
            dataDictionary.AddChild(PlatformPermissions.DataDictionary.Delete, L("Permission:Delete"));
            dataDictionary.AddChild(PlatformPermissions.DataDictionary.ManageItems, L("Permission:ManageItems"));

            var layout = platform.AddPermission(PlatformPermissions.Layout.Default, L("Permission:Layout"));
            layout.AddChild(PlatformPermissions.Layout.Create, L("Permission:Create"));
            layout.AddChild(PlatformPermissions.Layout.Update, L("Permission:Update"));
            layout.AddChild(PlatformPermissions.Layout.Delete, L("Permission:Delete"));

            var menu = platform.AddPermission(PlatformPermissions.Menu.Default, L("Permission:Menu"));
            menu.AddChild(PlatformPermissions.Menu.Create, L("Permission:Create"));
            menu.AddChild(PlatformPermissions.Menu.Update, L("Permission:Update"));
            menu.AddChild(PlatformPermissions.Menu.Delete, L("Permission:Delete"));
            menu.AddChild(PlatformPermissions.Menu.ManageRoles, L("Permission:ManageRoleMenus"));
            menu.AddChild(PlatformPermissions.Menu.ManageUsers, L("Permission:ManageUserMenus"));
            menu.AddChild(PlatformPermissions.Menu.ManageUserFavorites, L("Permission:ManageUserFavoriteMenus"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<PlatformResource>(name);
        }
    }
}
