namespace LINGYUN.Abp.Notifications.Permissions
{
    public class NotificationsPermissions
    {
        public const string GroupName = "Notifications";

        public class Notification
        {
            public const string Default = GroupName + ".Notification";

            public const string Delete = Default + ".Delete";
        }
    }
}
