namespace LINGYUN.Abp.MessageService.Permissions
{
    public static class MessageServicePermissions
    {
        public const string GroupName = "MessageService";

        public static class Notification
        {
            public const string Default = GroupName + ".Notification";

            public const string Delete = Default + ".Delete";
        }

        public static class Hangfire
        {
            public const string Default = GroupName + ".Hangfire";

            public const string Dashboard = Default + ".Dashboard";

            public const string ManageQueue = Default + ".ManageQueue";
        }
    }
}
