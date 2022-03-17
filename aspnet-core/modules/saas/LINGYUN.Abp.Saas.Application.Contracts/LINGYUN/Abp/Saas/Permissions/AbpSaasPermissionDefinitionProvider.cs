using LINGYUN.Abp.Saas.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Saas;

public class AbpSaasPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var saasGroup = context.AddGroup(AbpSaasPermissions.GroupName, L("Permission:Saas"));

        var editionsPermission = saasGroup.AddPermission(AbpSaasPermissions.Editions.Default, L("Permission:EditionManagement"), multiTenancySide: MultiTenancySides.Host);
        editionsPermission.AddChild(AbpSaasPermissions.Editions.Create, L("Permission:Create"), multiTenancySide: MultiTenancySides.Host);
        editionsPermission.AddChild(AbpSaasPermissions.Editions.Update, L("Permission:Edit"), multiTenancySide: MultiTenancySides.Host);
        editionsPermission.AddChild(AbpSaasPermissions.Editions.Delete, L("Permission:Delete"), multiTenancySide: MultiTenancySides.Host);
        editionsPermission.AddChild(AbpSaasPermissions.Editions.ManageFeatures, L("Permission:ManageFeatures"), multiTenancySide: MultiTenancySides.Host);

        var tenantsPermission = saasGroup.AddPermission(AbpSaasPermissions.Tenants.Default, L("Permission:TenantManagement"), multiTenancySide: MultiTenancySides.Host);
        tenantsPermission.AddChild(AbpSaasPermissions.Tenants.Create, L("Permission:Create"), multiTenancySide: MultiTenancySides.Host);
        tenantsPermission.AddChild(AbpSaasPermissions.Tenants.Update, L("Permission:Edit"), multiTenancySide: MultiTenancySides.Host);
        tenantsPermission.AddChild(AbpSaasPermissions.Tenants.Delete, L("Permission:Delete"), multiTenancySide: MultiTenancySides.Host);
        tenantsPermission.AddChild(AbpSaasPermissions.Tenants.ManageFeatures, L("Permission:ManageFeatures"), multiTenancySide: MultiTenancySides.Host);
        tenantsPermission.AddChild(AbpSaasPermissions.Tenants.ManageConnectionStrings, L("Permission:ManageConnectionStrings"), multiTenancySide: MultiTenancySides.Host);
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AbpSaasResource>(name);
    }
}
