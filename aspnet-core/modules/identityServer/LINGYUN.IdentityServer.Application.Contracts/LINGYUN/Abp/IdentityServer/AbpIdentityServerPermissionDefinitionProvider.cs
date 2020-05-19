using Volo.Abp.Authorization.Permissions;
using Volo.Abp.IdentityServer.Localization;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.IdentityServer
{
    public class AbpIdentityServerPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var identityServerGroup = context.AddGroup(AbpIdentityServerPermissions.GroupName, L("Permissions:IdentityServer"));

            // 客户端权限
            var clientPermissions = identityServerGroup.AddPermission(AbpIdentityServerPermissions.Clients.Default, L("Permissions:Clients"));
            clientPermissions.AddChild(AbpIdentityServerPermissions.Clients.Create, L("Permissions:Create"));
            clientPermissions.AddChild(AbpIdentityServerPermissions.Clients.Update, L("Permissions:Update"));
            clientPermissions.AddChild(AbpIdentityServerPermissions.Clients.Clone, L("Permissions:Clone"));
            clientPermissions.AddChild(AbpIdentityServerPermissions.Clients.Enabled, L("Permissions:Enabled"));
            clientPermissions.AddChild(AbpIdentityServerPermissions.Clients.Disabled, L("Permissions:Disabled"));
            clientPermissions.AddChild(AbpIdentityServerPermissions.Clients.Delete, L("Permissions:Delete"));
            clientPermissions.AddChild(AbpIdentityServerPermissions.Clients.ManagePermissions, L("Permissions:ManagePermissions"));

            // 客户端声明权限
            var clientClaimPermissiosn = clientPermissions.AddChild(AbpIdentityServerPermissions.Clients.Claims.Default, L("Permissions:Clients:Claims"));
            clientClaimPermissiosn.AddChild(AbpIdentityServerPermissions.Clients.Claims.Create, L("Permissions:Create"));
            clientClaimPermissiosn.AddChild(AbpIdentityServerPermissions.Clients.Claims.Update, L("Permissions:Update"));
            clientClaimPermissiosn.AddChild(AbpIdentityServerPermissions.Clients.Claims.Delete, L("Permissions:Delete"));

            // 客户端密钥权限
            var clientSecretPermissiosn = clientPermissions.AddChild(AbpIdentityServerPermissions.Clients.Secrets.Default, L("Permissions:Clients:Secrets"));
            clientSecretPermissiosn.AddChild(AbpIdentityServerPermissions.Clients.Secrets.Create, L("Permissions:Create"));
            clientSecretPermissiosn.AddChild(AbpIdentityServerPermissions.Clients.Secrets.Update, L("Permissions:Update"));
            clientSecretPermissiosn.AddChild(AbpIdentityServerPermissions.Clients.Secrets.Delete, L("Permissions:Delete"));

            // 客户端属性权限
            var clientPropertyPermissiosn = clientPermissions.AddChild(AbpIdentityServerPermissions.Clients.Properties.Default, L("Permissions:Clients:Properties"));
            clientPropertyPermissiosn.AddChild(AbpIdentityServerPermissions.Clients.Properties.Create, L("Permissions:Create"));
            clientPropertyPermissiosn.AddChild(AbpIdentityServerPermissions.Clients.Properties.Update, L("Permissions:Update"));
            clientPropertyPermissiosn.AddChild(AbpIdentityServerPermissions.Clients.Properties.Delete, L("Permissions:Delete"));

            // Api资源权限
            var apiResourcePermissions = identityServerGroup.AddPermission(AbpIdentityServerPermissions.ApiResources.Default, L("Permissions:ApiResources"));
            apiResourcePermissions.AddChild(AbpIdentityServerPermissions.ApiResources.Create, L("Permissions:Create"));
            apiResourcePermissions.AddChild(AbpIdentityServerPermissions.ApiResources.Update, L("Permissions:Update"));
            apiResourcePermissions.AddChild(AbpIdentityServerPermissions.ApiResources.Delete, L("Permissions:Delete"));

            // Api作用域权限
            var apiResourceScopePermissions = apiResourcePermissions.AddChild(AbpIdentityServerPermissions.ApiResources.Scope.Default, L("Permissions:ApiResources:Scope"));
            apiResourceScopePermissions.AddChild(AbpIdentityServerPermissions.ApiResources.Scope.Create, L("Permissions:Create"));
            apiResourceScopePermissions.AddChild(AbpIdentityServerPermissions.ApiResources.Scope.Update, L("Permissions:Update"));
            apiResourceScopePermissions.AddChild(AbpIdentityServerPermissions.ApiResources.Scope.Delete, L("Permissions:Delete"));

            // Api密钥权限
            var apiResourceSecretPermissions = apiResourcePermissions.AddChild(AbpIdentityServerPermissions.ApiResources.Secrets.Default, L("Permissions:ApiResources:Secrets"));
            apiResourceSecretPermissions.AddChild(AbpIdentityServerPermissions.ApiResources.Secrets.Create, L("Permissions:Create"));
            apiResourceSecretPermissions.AddChild(AbpIdentityServerPermissions.ApiResources.Secrets.Update, L("Permissions:Update"));
            apiResourceSecretPermissions.AddChild(AbpIdentityServerPermissions.ApiResources.Secrets.Delete, L("Permissions:Delete"));
        }

        protected virtual LocalizableString L(string name)
        {
            return LocalizableString.Create<AbpIdentityServerResource>(name);
        }
    }
}
