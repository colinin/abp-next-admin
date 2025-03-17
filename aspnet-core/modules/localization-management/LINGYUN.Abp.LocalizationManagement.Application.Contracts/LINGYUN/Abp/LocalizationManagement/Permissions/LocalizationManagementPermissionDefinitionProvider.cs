using LINGYUN.Abp.LocalizationManagement.Features;
using LINGYUN.Abp.LocalizationManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Features;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.LocalizationManagement.Permissions;

public class LocalizationManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var permissionGroup = context.AddGroup(
            LocalizationManagementPermissions.GroupName,
            L("Permissions:LocalizationManagement"));

        var resourcePermission = permissionGroup.AddPermission(
            LocalizationManagementPermissions.Resource.Default,
            L("Permissions:Resource"),
            Volo.Abp.MultiTenancy.MultiTenancySides.Host)
            .RequireFeatures(LocalizationManagementFeatures.Enable);
        resourcePermission.AddChild(
            LocalizationManagementPermissions.Resource.Create,
            L("Permissions:Create"),
            Volo.Abp.MultiTenancy.MultiTenancySides.Host)
            .RequireFeatures(LocalizationManagementFeatures.Enable);
        resourcePermission.AddChild(
            LocalizationManagementPermissions.Resource.Update,
            L("Permissions:Update"),
            Volo.Abp.MultiTenancy.MultiTenancySides.Host)
            .RequireFeatures(LocalizationManagementFeatures.Enable);
        resourcePermission.AddChild(
            LocalizationManagementPermissions.Resource.Delete,
            L("Permissions:Delete"),
            Volo.Abp.MultiTenancy.MultiTenancySides.Host)
            .RequireFeatures(LocalizationManagementFeatures.Enable);

        var languagePermission = permissionGroup.AddPermission(
            LocalizationManagementPermissions.Language.Default,
            L("Permissions:Language"),
            Volo.Abp.MultiTenancy.MultiTenancySides.Host)
            .RequireFeatures(LocalizationManagementFeatures.Enable);
        languagePermission.AddChild(
            LocalizationManagementPermissions.Language.Create,
            L("Permissions:Create"),
            Volo.Abp.MultiTenancy.MultiTenancySides.Host)
            .RequireFeatures(LocalizationManagementFeatures.Enable);
        languagePermission.AddChild(
            LocalizationManagementPermissions.Language.Update,
            L("Permissions:Update"),
            Volo.Abp.MultiTenancy.MultiTenancySides.Host)
            .RequireFeatures(LocalizationManagementFeatures.Enable);
        languagePermission.AddChild(
            LocalizationManagementPermissions.Language.Delete,
            L("Permissions:Delete"),
            Volo.Abp.MultiTenancy.MultiTenancySides.Host)
            .RequireFeatures(LocalizationManagementFeatures.Enable);

        var textPermission = permissionGroup.AddPermission(
            LocalizationManagementPermissions.Text.Default,
            L("Permissions:Text"),
            Volo.Abp.MultiTenancy.MultiTenancySides.Host)
            .RequireFeatures(LocalizationManagementFeatures.Enable);
        textPermission.AddChild(
            LocalizationManagementPermissions.Text.Create,
            L("Permissions:Create"),
            Volo.Abp.MultiTenancy.MultiTenancySides.Host)
            .RequireFeatures(LocalizationManagementFeatures.Enable);
        textPermission.AddChild(
            LocalizationManagementPermissions.Text.Update,
            L("Permissions:Update"),
            Volo.Abp.MultiTenancy.MultiTenancySides.Host)
            .RequireFeatures(LocalizationManagementFeatures.Enable);
        textPermission.AddChild(
            LocalizationManagementPermissions.Text.Delete,
            L("Permissions:Delete"),
            Volo.Abp.MultiTenancy.MultiTenancySides.Host)
            .RequireFeatures(LocalizationManagementFeatures.Enable);
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<LocalizationManagementResource>(name);
    }
}
