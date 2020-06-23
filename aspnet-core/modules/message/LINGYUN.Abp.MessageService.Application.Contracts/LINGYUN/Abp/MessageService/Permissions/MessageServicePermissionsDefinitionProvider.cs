using LINGYUN.Abp.MessageService.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.MessageService.Permissions
{
    public class MessageServicePermissionsDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var group = context.AddGroup(MessageServicePermissions.GroupName, L("Permission:MessageService"));

            var noticeGroup = group.AddPermission(MessageServicePermissions.Notification.Default, L("Permission:Notification"));
            noticeGroup.AddChild(MessageServicePermissions.Notification.Delete, L("Permission:Delete"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<MessageServiceResource>(name);
        }
    }
}
