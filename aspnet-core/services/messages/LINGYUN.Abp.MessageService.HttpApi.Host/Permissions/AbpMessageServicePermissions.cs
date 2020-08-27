namespace LINGYUN.Abp.MessageService.Permissions
{
    public class AbpMessageServicePermissions
    {
        public class Hangfire
        {
            public const string Default = MessageServicePermissions.GroupName + ".Hangfire";

            public const string ManageQueue = Default + ".ManageQueue";
        }
    }
}
