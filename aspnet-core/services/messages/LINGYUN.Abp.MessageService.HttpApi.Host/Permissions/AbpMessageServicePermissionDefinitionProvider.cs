using LINGYUN.Abp.MessageService.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.MessageService.Permissions
{
    public class AbpMessageServicePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var group = context.GetGroup(MessageServicePermissions.GroupName);

            var hangfirePermission = group.AddPermission(AbpMessageServicePermissions.Hangfire.Default, L("Permission:Hangfire"));
            hangfirePermission.AddChild(AbpMessageServicePermissions.Hangfire.ManageQueue, L("Permission:ManageQueue"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<MessageServiceResource>(name);
        }
    }
}
