using LINGYUN.Abp.OssManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.OssManagement.Permissions
{
    public class AbpOssManagementPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var ossManagement = context.AddGroup(AbpOssManagementPermissions.GroupName, L("Permission:OssManagement"));

            var container = ossManagement.AddPermission(AbpOssManagementPermissions.Container.Default, L("Permission:Container"));
            container.AddChild(AbpOssManagementPermissions.Container.Create, L("Permission:Create"));
            container.AddChild(AbpOssManagementPermissions.Container.Delete, L("Permission:Delete"));

            var ossobject = ossManagement.AddPermission(AbpOssManagementPermissions.OssObject.Default, L("Permission:OssObject"));
            ossobject.AddChild(AbpOssManagementPermissions.OssObject.Create, L("Permission:Create"));
            ossobject.AddChild(AbpOssManagementPermissions.OssObject.Delete, L("Permission:Delete"));
            ossobject.AddChild(AbpOssManagementPermissions.OssObject.Download, L("Permission:Download"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<AbpOssManagementResource>(name);
        }
    }
}
