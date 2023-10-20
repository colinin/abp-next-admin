using Volo.Abp;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.FeatureManagement.Localization;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.FeatureManagement.Client.Permissions
{
    public class ClientFeaturePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            // TODO: 硬编码权限名称还是引用 Volo.Abp.FeatureManagement.Application.Contracts?

            var identityServerGroup = context.GetGroupOrNull(ClientFeaturePermissionNames.GroupName);
            Check.NotNull(identityServerGroup, $"Permissions:{ClientFeaturePermissionNames.GroupName}");

            identityServerGroup
                .AddPermission(
                    name: ClientFeaturePermissionNames.ManageClientFeatures,
                    displayName: L("Permissions:ManageClientFeatures"),
                    multiTenancySide: Volo.Abp.MultiTenancy.MultiTenancySides.Host);
        }

        protected virtual LocalizableString L(string name)
        {
            return LocalizableString.Create<AbpFeatureManagementResource>(name);
        }
    }
}
