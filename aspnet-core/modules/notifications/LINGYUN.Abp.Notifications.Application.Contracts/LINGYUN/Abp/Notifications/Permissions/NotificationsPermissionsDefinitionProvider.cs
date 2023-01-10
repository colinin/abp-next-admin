using LINGYUN.Abp.Notifications.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Notifications.Permissions
{
    public class NotificationsPermissionsDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var group = context.AddGroup(NotificationsPermissions.GroupName, L("Permission:Notifications"));

            var noticeGroup = group.AddPermission(NotificationsPermissions.Notification.Default, L("Permission:Notification"));
            noticeGroup.AddChild(NotificationsPermissions.Notification.Delete, L("Permission:Delete"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<NotificationsResource>(name);
        }
    }
}
