using Volo.Abp.Authorization.Permissions;
using Volo.Abp.IdentityServer.Localization;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.IdentityServer
{
    public class AbpIdentityServerPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            // TODO: 身份认证服务器应该只能主机管辖
            // 增加 MultiTenancySides.Host
            // var identityServerGroup = context.AddGroup(AbpIdentityServerPermissions.GroupName, L("Permissions:IdentityServer"), MultiTenancySides.Host);

            // 与 LINGYUN.Abp.FeatureManagement.Client 模块搭配,这样干可以不依赖于模块优先级
            var identityServerGroup = context.GetGroupOrNull(AbpIdentityServerPermissions.GroupName);
            if (identityServerGroup == null)
            {
                identityServerGroup = context
                    .AddGroup(
                        name: AbpIdentityServerPermissions.GroupName,
                        displayName: L("Permissions:IdentityServer"),
                        multiTenancySide: MultiTenancySides.Host);
            }
            // 客户端权限
            var clientPermissions = identityServerGroup.AddPermission(AbpIdentityServerPermissions.Clients.Default, L("Permissions:Clients"), MultiTenancySides.Host);
            clientPermissions.AddChild(AbpIdentityServerPermissions.Clients.Create, L("Permissions:Create"), MultiTenancySides.Host);
            clientPermissions.AddChild(AbpIdentityServerPermissions.Clients.Update, L("Permissions:Update"), MultiTenancySides.Host);
            clientPermissions.AddChild(AbpIdentityServerPermissions.Clients.Clone, L("Permissions:Clone"), MultiTenancySides.Host);
            clientPermissions.AddChild(AbpIdentityServerPermissions.Clients.Enabled, L("Permissions:Enabled"), MultiTenancySides.Host);
            clientPermissions.AddChild(AbpIdentityServerPermissions.Clients.Disabled, L("Permissions:Disabled"), MultiTenancySides.Host);
            clientPermissions.AddChild(AbpIdentityServerPermissions.Clients.Delete, L("Permissions:Delete"), MultiTenancySides.Host);
            clientPermissions.AddChild(AbpIdentityServerPermissions.Clients.ManagePermissions, L("Permissions:ManagePermissions"), MultiTenancySides.Host);

            // 客户端声明权限
            var clientClaimPermissiosn = clientPermissions.AddChild(AbpIdentityServerPermissions.Clients.Claims.Default, L("Permissions:Clients:Claims"), MultiTenancySides.Host);
            clientClaimPermissiosn.AddChild(AbpIdentityServerPermissions.Clients.Claims.Create, L("Permissions:Create"), MultiTenancySides.Host);
            clientClaimPermissiosn.AddChild(AbpIdentityServerPermissions.Clients.Claims.Update, L("Permissions:Update"), MultiTenancySides.Host);
            clientClaimPermissiosn.AddChild(AbpIdentityServerPermissions.Clients.Claims.Delete, L("Permissions:Delete"), MultiTenancySides.Host);

            // 客户端密钥权限
            var clientSecretPermissiosn = clientPermissions.AddChild(AbpIdentityServerPermissions.Clients.Secrets.Default, L("Permissions:Clients:Secrets"), MultiTenancySides.Host);
            clientSecretPermissiosn.AddChild(AbpIdentityServerPermissions.Clients.Secrets.Create, L("Permissions:Create"), MultiTenancySides.Host);
            clientSecretPermissiosn.AddChild(AbpIdentityServerPermissions.Clients.Secrets.Update, L("Permissions:Update"), MultiTenancySides.Host);
            clientSecretPermissiosn.AddChild(AbpIdentityServerPermissions.Clients.Secrets.Delete, L("Permissions:Delete"), MultiTenancySides.Host);

            // 客户端属性权限
            var clientPropertyPermissiosn = clientPermissions.AddChild(AbpIdentityServerPermissions.Clients.Properties.Default, L("Permissions:Clients:Properties"), MultiTenancySides.Host);
            clientPropertyPermissiosn.AddChild(AbpIdentityServerPermissions.Clients.Properties.Create, L("Permissions:Create"), MultiTenancySides.Host);
            clientPropertyPermissiosn.AddChild(AbpIdentityServerPermissions.Clients.Properties.Update, L("Permissions:Update"), MultiTenancySides.Host);
            clientPropertyPermissiosn.AddChild(AbpIdentityServerPermissions.Clients.Properties.Delete, L("Permissions:Delete"), MultiTenancySides.Host);

            // Api资源权限
            var apiResourcePermissions = identityServerGroup.AddPermission(AbpIdentityServerPermissions.ApiResources.Default, L("Permissions:ApiResources"), MultiTenancySides.Host);
            apiResourcePermissions.AddChild(AbpIdentityServerPermissions.ApiResources.Create, L("Permissions:Create"), MultiTenancySides.Host);
            apiResourcePermissions.AddChild(AbpIdentityServerPermissions.ApiResources.Update, L("Permissions:Update"), MultiTenancySides.Host);
            apiResourcePermissions.AddChild(AbpIdentityServerPermissions.ApiResources.Delete, L("Permissions:Delete"), MultiTenancySides.Host);

            // Api作用域权限
            var apiResourceScopePermissions = apiResourcePermissions.AddChild(AbpIdentityServerPermissions.ApiResources.Scope.Default, L("Permissions:ApiResources:Scope"), MultiTenancySides.Host);
            apiResourceScopePermissions.AddChild(AbpIdentityServerPermissions.ApiResources.Scope.Create, L("Permissions:Create"), MultiTenancySides.Host);
            apiResourceScopePermissions.AddChild(AbpIdentityServerPermissions.ApiResources.Scope.Update, L("Permissions:Update"), MultiTenancySides.Host);
            apiResourceScopePermissions.AddChild(AbpIdentityServerPermissions.ApiResources.Scope.Delete, L("Permissions:Delete"), MultiTenancySides.Host);

            // Api密钥权限
            var apiResourceSecretPermissions = apiResourcePermissions.AddChild(AbpIdentityServerPermissions.ApiResources.Secrets.Default, L("Permissions:ApiResources:Secrets"), MultiTenancySides.Host);
            apiResourceSecretPermissions.AddChild(AbpIdentityServerPermissions.ApiResources.Secrets.Create, L("Permissions:Create"), MultiTenancySides.Host);
            apiResourceSecretPermissions.AddChild(AbpIdentityServerPermissions.ApiResources.Secrets.Update, L("Permissions:Update"), MultiTenancySides.Host);
            apiResourceSecretPermissions.AddChild(AbpIdentityServerPermissions.ApiResources.Secrets.Delete, L("Permissions:Delete"), MultiTenancySides.Host);

            // 身份资源权限
            var identityResourcePermissions = identityServerGroup.AddPermission(AbpIdentityServerPermissions.IdentityResources.Default, L("Permissions:IdentityResources"), MultiTenancySides.Host);
            identityResourcePermissions.AddChild(AbpIdentityServerPermissions.IdentityResources.Create, L("Permissions:Create"), MultiTenancySides.Host);
            identityResourcePermissions.AddChild(AbpIdentityServerPermissions.IdentityResources.Update, L("Permissions:Update"), MultiTenancySides.Host);
            identityResourcePermissions.AddChild(AbpIdentityServerPermissions.IdentityResources.Delete, L("Permissions:Delete"), MultiTenancySides.Host);

            // 身份资源属性权限
            var identityResourcePropertyPermissiosn = identityResourcePermissions.AddChild(AbpIdentityServerPermissions.IdentityResources.Properties.Default, L("Permissions:IdentityResources:Properties"), MultiTenancySides.Host);
            identityResourcePropertyPermissiosn.AddChild(AbpIdentityServerPermissions.IdentityResources.Properties.Create, L("Permissions:Create"), MultiTenancySides.Host);
            identityResourcePropertyPermissiosn.AddChild(AbpIdentityServerPermissions.IdentityResources.Properties.Delete, L("Permissions:Delete"), MultiTenancySides.Host);
        }

        protected virtual LocalizableString L(string name)
        {
            return LocalizableString.Create<AbpIdentityServerResource>(name);
        }
    }
}
