using LINGYUN.Abp.BlobManagement.Features;
using LINGYUN.Abp.BlobManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Features;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.BlobManagement.Permissions;

public class BlobManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var blobManagement = context.AddGroup(BlobManagementPermissionNames.GroupName, L("Permission:BlobManagement"));

        var container = blobManagement.AddPermission(
            BlobManagementPermissionNames.BlobContainer.Default,
            L("Permission:BlobContainer"));

        container.AddChild(BlobManagementPermissionNames.BlobContainer.Create, L("Permission:Create"));
        container.AddChild(BlobManagementPermissionNames.BlobContainer.Delete, L("Permission:Delete"));

        var blob = blobManagement
            .AddPermission(BlobManagementPermissionNames.Blob.Default, L("Permission:Blob"))
            .RequireFeatures(BlobManagementFeatureNames.Blob.Enable);
        blob.AddChild(BlobManagementPermissionNames.Blob.Create, L("Permission:Upload"))
            .RequireFeatures(BlobManagementFeatureNames.Blob.UploadFile);
        blob.AddChild(BlobManagementPermissionNames.Blob.Delete, L("Permission:Delete"));
        blob.AddChild(BlobManagementPermissionNames.Blob.Download, L("Permission:Download"))
            .RequireFeatures(BlobManagementFeatureNames.Blob.DownloadFile);

        context.AddResourcePermission(
            name: BlobManagementPermissionNames.Blob.Resources.Delete,
            resourceName: BlobManagementPermissionNames.Blob.Resources.Name,
            managementPermissionName: BlobManagementPermissionNames.Blob.ManagePermissions,
            displayName: L("Permission:Blob:Delete")
        );
        context.AddResourcePermission(
            name: BlobManagementPermissionNames.Blob.Resources.View,
            resourceName: BlobManagementPermissionNames.Blob.Resources.Name,
            managementPermissionName: BlobManagementPermissionNames.Blob.ManagePermissions,
            displayName: L("Permission:Blob:View")
        );
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<BlobManagementResource>(name);
    }
}
