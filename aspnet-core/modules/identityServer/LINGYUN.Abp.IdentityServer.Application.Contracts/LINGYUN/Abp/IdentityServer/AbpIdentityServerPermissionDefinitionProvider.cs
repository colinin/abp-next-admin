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
            clientPermissions.AddChild(AbpIdentityServerPermissions.Clients.Delete, L("Permissions:Delete"), MultiTenancySides.Host);
            clientPermissions.AddChild(AbpIdentityServerPermissions.Clients.ManagePermissions, L("Permissions:ManagePermissions"), MultiTenancySides.Host);
            clientPermissions.AddChild(AbpIdentityServerPermissions.Clients.ManageClaims, L("Permissions:ManageClaims"), MultiTenancySides.Host);
            clientPermissions.AddChild(AbpIdentityServerPermissions.Clients.ManageSecrets, L("Permissions:ManageSecrets"), MultiTenancySides.Host);
            clientPermissions.AddChild(AbpIdentityServerPermissions.Clients.ManageProperties, L("Permissions:ManageProperties"), MultiTenancySides.Host);

            // Api资源权限
            var apiResourcePermissions = identityServerGroup.AddPermission(AbpIdentityServerPermissions.ApiResources.Default, L("Permissions:ApiResources"), MultiTenancySides.Host);
            apiResourcePermissions.AddChild(AbpIdentityServerPermissions.ApiResources.Create, L("Permissions:Create"), MultiTenancySides.Host);
            apiResourcePermissions.AddChild(AbpIdentityServerPermissions.ApiResources.Update, L("Permissions:Update"), MultiTenancySides.Host);
            apiResourcePermissions.AddChild(AbpIdentityServerPermissions.ApiResources.Delete, L("Permissions:Delete"), MultiTenancySides.Host);
            apiResourcePermissions.AddChild(AbpIdentityServerPermissions.ApiResources.ManageClaims, L("Permissions:ManageClaims"), MultiTenancySides.Host);
            apiResourcePermissions.AddChild(AbpIdentityServerPermissions.ApiResources.ManageSecrets, L("Permissions:ManageSecrets"), MultiTenancySides.Host);
            apiResourcePermissions.AddChild(AbpIdentityServerPermissions.ApiResources.ManageScopes, L("Permissions:ManageScopes"), MultiTenancySides.Host);
            apiResourcePermissions.AddChild(AbpIdentityServerPermissions.ApiResources.ManageProperties, L("Permissions:ManageProperties"), MultiTenancySides.Host);

            // Api范围权限
            var apiScopePermissions = identityServerGroup.AddPermission(AbpIdentityServerPermissions.ApiScopes.Default, L("Permissions:ApiScopes"), MultiTenancySides.Host);
            apiScopePermissions.AddChild(AbpIdentityServerPermissions.ApiScopes.Create, L("Permissions:Create"), MultiTenancySides.Host);
            apiScopePermissions.AddChild(AbpIdentityServerPermissions.ApiScopes.Update, L("Permissions:Update"), MultiTenancySides.Host);
            apiScopePermissions.AddChild(AbpIdentityServerPermissions.ApiScopes.Delete, L("Permissions:Delete"), MultiTenancySides.Host);
            apiScopePermissions.AddChild(AbpIdentityServerPermissions.ApiScopes.ManageClaims, L("Permissions:ManageClaims"), MultiTenancySides.Host);
            apiScopePermissions.AddChild(AbpIdentityServerPermissions.ApiScopes.ManageProperties, L("Permissions:ManageProperties"), MultiTenancySides.Host);

            // 身份资源权限
            var identityResourcePermissions = identityServerGroup.AddPermission(AbpIdentityServerPermissions.IdentityResources.Default, L("Permissions:IdentityResources"), MultiTenancySides.Host);
            identityResourcePermissions.AddChild(AbpIdentityServerPermissions.IdentityResources.Create, L("Permissions:Create"), MultiTenancySides.Host);
            identityResourcePermissions.AddChild(AbpIdentityServerPermissions.IdentityResources.Update, L("Permissions:Update"), MultiTenancySides.Host);
            identityResourcePermissions.AddChild(AbpIdentityServerPermissions.IdentityResources.Delete, L("Permissions:Delete"), MultiTenancySides.Host);
            identityResourcePermissions.AddChild(AbpIdentityServerPermissions.IdentityResources.ManageClaims, L("Permissions:ManageClaims"), MultiTenancySides.Host);
            identityResourcePermissions.AddChild(AbpIdentityServerPermissions.IdentityResources.ManageProperties, L("Permissions:ManageProperties"), MultiTenancySides.Host);

            // 持久授权
            var persistedGrantPermissions = identityServerGroup.AddPermission(AbpIdentityServerPermissions.Grants.Default, L("Permissions:Grants"), MultiTenancySides.Host);
            persistedGrantPermissions.AddChild(AbpIdentityServerPermissions.Grants.Delete, L("Permissions:Delete"), MultiTenancySides.Host);
        }

        protected virtual LocalizableString L(string name)
        {
            return LocalizableString.Create<AbpIdentityServerResource>(name);
        }
    }
}
