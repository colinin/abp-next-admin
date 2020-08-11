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
