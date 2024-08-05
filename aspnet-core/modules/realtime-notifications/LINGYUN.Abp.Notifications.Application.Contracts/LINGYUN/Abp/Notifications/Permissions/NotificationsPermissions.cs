namespace LINGYUN.Abp.Notifications.Permissions;

public class NotificationsPermissions
{
    public const string GroupName = "Notifications";

    public static class Notification
    {
        public const string Default = GroupName + ".Notification";

        public const string Delete = Default + ".Delete";
    }

    public static class GroupDefinition
    {
        public const string Default = GroupName + ".GroupDefinitions";

        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class Definition
    {
        public const string Default = GroupName + ".Definitions";

        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }
}
