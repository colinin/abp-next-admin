using Volo.Abp.Authorization.Permissions;
using Volo.Abp.FeatureManagement.Localization;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.FeatureManagement.Client.Permissions
{
    public class ClientFeaturePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var identityServerGroup = context.GetGroupOrNull(ClientFeaturePermissionNames.GroupName);
            if (identityServerGroup == null)
            {
                identityServerGroup = context
                    .AddGroup(
                        name: ClientFeaturePermissionNames.GroupName,
                        displayName: L("Permissions:IdentityServer"),
                        multiTenancySide: Volo.Abp.MultiTenancy.MultiTenancySides.Host);
            }
            identityServerGroup
                .AddPermission(
                    name: ClientFeaturePermissionNames.Clients.ManageFeatures,
                    displayName: L("Permissions:ManageFeatures"),
                    multiTenancySide: Volo.Abp.MultiTenancy.MultiTenancySides.Host)
                .WithProviders(ClientPermissionValueProvider.ProviderName);
        }

        protected virtual LocalizableString L(string name)
        {
            return LocalizableString.Create<AbpFeatureManagementResource>(name);
        }
    }
}
