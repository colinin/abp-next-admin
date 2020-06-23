namespace LINGYUN.Abp.MessageService.Permissions
{
    public class MessageServicePermissions
    {
        public const string GroupName = "MessageService";

        public class Notification
        {
            public const string Default = GroupName + ".Notification";

            public const string Delete = Default + ".Delete";
        }
    }
}
