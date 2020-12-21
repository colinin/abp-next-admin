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


            // TODO: 2020-07-27 目前abp不支持对象存储管理（或者属于企业版?）需要创建一个 LINGYUN.Abp.BlobStoring 项目自行实现

            //var fileSystem = platform.AddPermission(PlatformPermissions.FileSystem.Default, L("Permission:FileSystem"));
            //fileSystem.AddChild(PlatformPermissions.FileSystem.Create, L("Permission:CreateFolder"));
            //fileSystem.AddChild(PlatformPermissions.FileSystem.Delete, L("Permission:DeleteFolder"));
            //fileSystem.AddChild(PlatformPermissions.FileSystem.Rename, L("Permission:RenameFolder"));
            //fileSystem.AddChild(PlatformPermissions.FileSystem.Copy, L("Permission:CopyFolder"));
            //fileSystem.AddChild(PlatformPermissions.FileSystem.Move, L("Permission:MoveFolder"));

            //var fileManager = fileSystem.AddChild(PlatformPermissions.FileSystem.FileManager.Default, L("Permission:FileManager"));
            //fileManager.AddChild(PlatformPermissions.FileSystem.FileManager.Create, L("Permission:AppendFile"));
            //fileManager.AddChild(PlatformPermissions.FileSystem.FileManager.Update, L("Permission:UpdateFile"));
            //fileManager.AddChild(PlatformPermissions.FileSystem.FileManager.Delete, L("Permission:DeleteFile"));
            //fileManager.AddChild(PlatformPermissions.FileSystem.FileManager.Copy, L("Permission:CopyFile"));
            //fileManager.AddChild(PlatformPermissions.FileSystem.FileManager.Move, L("Permission:MoveFile"));
            //fileManager.AddChild(PlatformPermissions.FileSystem.FileManager.Download, L("Permission:DownloadFile"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<PlatformResource>(name);
        }
    }
}
