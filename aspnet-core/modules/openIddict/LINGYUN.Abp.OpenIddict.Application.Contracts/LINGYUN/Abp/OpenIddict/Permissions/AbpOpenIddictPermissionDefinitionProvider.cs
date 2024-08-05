using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.OpenIddict.Localization;

namespace LINGYUN.Abp.OpenIddict.Permissions;

public class AbpIdentityServerPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var openIddictGroup = context.GetGroupOrNull(AbpOpenIddictPermissions.GroupName);
        if (openIddictGroup == null)
        {
            openIddictGroup = context
                .AddGroup(
                    name: AbpOpenIddictPermissions.GroupName,
                    displayName: L("Permissions:OpenIddict"));
        }

        var applications = openIddictGroup.AddPermission(
            AbpOpenIddictPermissions.Applications.Default,
            L("Permissions:Applications"),
            MultiTenancySides.Host);
        applications.AddChild(
            AbpOpenIddictPermissions.Applications.Create,
            L("Permissions:Create"), 
            MultiTenancySides.Host);
        applications.AddChild(
            AbpOpenIddictPermissions.Applications.Update, 
            L("Permissions:Update"), 
            MultiTenancySides.Host);
        applications.AddChild(
            AbpOpenIddictPermissions.Applications.Delete, 
            L("Permissions:Delete"), 
            MultiTenancySides.Host);
        applications.AddChild(
            AbpOpenIddictPermissions.Applications.ManagePermissions, 
            L("Permissions:ManagePermissions"), 
            MultiTenancySides.Host);
        applications.AddChild(
            AbpOpenIddictPermissions.Applications.ManageSecret,
            L("Permissions:ManageSecret"),
            MultiTenancySides.Host);

        var authorizations = openIddictGroup.AddPermission(
            AbpOpenIddictPermissions.Authorizations.Default,
            L("Permissions:Authorizations"),
            MultiTenancySides.Host);
        authorizations.AddChild(
            AbpOpenIddictPermissions.Authorizations.Delete,
            L("Permissions:Delete"),
            MultiTenancySides.Host);

        var scopes = openIddictGroup.AddPermission(
            AbpOpenIddictPermissions.Scopes.Default,
            L("Permissions:Scopes"),
            MultiTenancySides.Host);
        scopes.AddChild(
            AbpOpenIddictPermissions.Scopes.Create,
            L("Permissions:Create"),
            MultiTenancySides.Host);
        scopes.AddChild(
            AbpOpenIddictPermissions.Scopes.Update,
            L("Permissions:Update"),
            MultiTenancySides.Host);
        scopes.AddChild(
            AbpOpenIddictPermissions.Scopes.Delete,
            L("Permissions:Delete"),
            MultiTenancySides.Host);

        var tokens = openIddictGroup.AddPermission(
            AbpOpenIddictPermissions.Tokens.Default,
            L("Permissions:Tokens"),
            MultiTenancySides.Host);
        tokens.AddChild(
            AbpOpenIddictPermissions.Tokens.Delete,
            L("Permissions:Delete"),
            MultiTenancySides.Host);
    }

    protected virtual LocalizableString L(string name)
    {
        return LocalizableString.Create<AbpOpenIddictResource>(name);
    }
}
