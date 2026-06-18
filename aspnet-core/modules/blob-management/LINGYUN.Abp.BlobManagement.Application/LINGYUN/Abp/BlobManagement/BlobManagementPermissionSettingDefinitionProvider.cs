using LINGYUN.Abp.BlobManagement.Permissions;
using LINGYUN.Abp.BlobManagement.Settings;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.BlobManagement;

public class BlobManagementPermissionSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        context.GetOrNull(BlobManagementSettingNames.GenerateDownloadUrlExpirySeconds)
            ?.RequiredGroupPermissions([BlobManagementPermissionNames.Blob.Default]);
        context.GetOrNull(BlobManagementSettingNames.FileLimitLength)
            ?.RequiredGroupPermissions([BlobManagementPermissionNames.Blob.Default]);
        context.GetOrNull(BlobManagementSettingNames.AllowFileExtensions)
            ?.RequiredGroupPermissions([BlobManagementPermissionNames.Blob.Default]);
    }
}
