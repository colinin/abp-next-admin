using LINGYUN.Abp.Hangfire.Dashboard.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Hangfire.Dashboard.Authorization
{
    public class HangfireDashboardPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var group = context.AddGroup(
                HangfireDashboardPermissions.GroupName,
                L("Permission:Hangfire"),
                MultiTenancySides.Host); // 除非对Hangfire Api进行改造,否则不能区分租户

            var dashboard = group.AddPermission(
                HangfireDashboardPermissions.Dashboard.Default,
                L("Permission:Dashboard"),
                MultiTenancySides.Host);

            dashboard.AddChild(
                HangfireDashboardPermissions.Dashboard.ManageJobs,
                L("Permission:ManageJobs"),
                MultiTenancySides.Host);
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<HangfireDashboardResource>(name);
        }
    }
}
