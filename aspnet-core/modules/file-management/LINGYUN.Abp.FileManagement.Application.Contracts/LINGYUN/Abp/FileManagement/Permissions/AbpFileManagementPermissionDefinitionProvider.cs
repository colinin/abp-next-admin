using LINGYUN.Abp.FileManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.FileManagement.Permissions
{
    public class AbpFileManagementPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var fileManagement = context.AddGroup(AbpFileManagementPermissions.GroupName, L("Permission:FileManagement"));

            var fileSystem = fileManagement.AddPermission(AbpFileManagementPermissions.FileSystem.Default, L("Permission:FileSystem"));
            fileSystem.AddChild(AbpFileManagementPermissions.FileSystem.Create, L("Permission:CreateFolder"));
            fileSystem.AddChild(AbpFileManagementPermissions.FileSystem.Delete, L("Permission:DeleteFolder"));
            fileSystem.AddChild(AbpFileManagementPermissions.FileSystem.Update, L("Permission:UpdateFolder"));
            fileSystem.AddChild(AbpFileManagementPermissions.FileSystem.Copy, L("Permission:CopyFolder"));
            fileSystem.AddChild(AbpFileManagementPermissions.FileSystem.Move, L("Permission:MoveFolder"));

            var fileManager = fileSystem.AddChild(AbpFileManagementPermissions.FileSystem.FileManager.Default, L("Permission:FileManager"));
            fileManager.AddChild(AbpFileManagementPermissions.FileSystem.FileManager.Create, L("Permission:AppendFile"));
            fileManager.AddChild(AbpFileManagementPermissions.FileSystem.FileManager.Update, L("Permission:UpdateFile"));
            fileManager.AddChild(AbpFileManagementPermissions.FileSystem.FileManager.Delete, L("Permission:DeleteFile"));
            fileManager.AddChild(AbpFileManagementPermissions.FileSystem.FileManager.Copy, L("Permission:CopyFile"));
            fileManager.AddChild(AbpFileManagementPermissions.FileSystem.FileManager.Move, L("Permission:MoveFile"));
            fileManager.AddChild(AbpFileManagementPermissions.FileSystem.FileManager.Download, L("Permission:DownloadFile"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<AbpFileManagementResource>(name);
        }
    }
}
