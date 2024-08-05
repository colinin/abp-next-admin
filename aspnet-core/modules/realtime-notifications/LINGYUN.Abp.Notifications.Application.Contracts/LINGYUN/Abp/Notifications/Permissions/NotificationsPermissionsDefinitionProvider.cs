using LINGYUN.Abp.Notifications.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Notifications.Permissions;

public class NotificationsPermissionsDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var group = context.AddGroup(NotificationsPermissions.GroupName, L("Permission:Notifications"));

        var groupDefinition = group.AddPermission(
            NotificationsPermissions.GroupDefinition.Default,
            L("Permission:GroupDefinitions"),
            MultiTenancySides.Host);
        groupDefinition.AddChild(
            NotificationsPermissions.GroupDefinition.Create,
            L("Permission:Create"),
            MultiTenancySides.Host);
        groupDefinition.AddChild(
            NotificationsPermissions.GroupDefinition.Update,
            L("Permission:Edit"),
            MultiTenancySides.Host);
        groupDefinition.AddChild(
            NotificationsPermissions.GroupDefinition.Delete,
            L("Permission:Delete"),
            MultiTenancySides.Host);

        var definition = group.AddPermission(
            NotificationsPermissions.Definition.Default,
            L("Permission:NotificationDefinitions"),
            MultiTenancySides.Host);
        definition.AddChild(
            NotificationsPermissions.Definition.Create,
            L("Permission:Create"),
            MultiTenancySides.Host);
        definition.AddChild(
            NotificationsPermissions.Definition.Update,
            L("Permission:Edit"),
            MultiTenancySides.Host);
        definition.AddChild(
            NotificationsPermissions.Definition.Delete,
            L("Permission:Delete"),
            MultiTenancySides.Host);

        var noticeGroup = group.AddPermission(NotificationsPermissions.Notification.Default, L("Permission:Notification"));
        noticeGroup.AddChild(NotificationsPermissions.Notification.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<NotificationsResource>(name);
    }
}
