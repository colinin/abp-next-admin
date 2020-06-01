namespace LINGYUN.Abp.MessageService
{
    public class MessageServiceSettingNames
    {
        public const string GroupName = "Abp.MessageService";

        public class Notifications
        {
            public const string Default = GroupName + ".Notifications";
            /// <summary>
            /// 清理过期消息批次
            /// </summary>
            public const string CleanupExpirationBatchCount = Default + ".CleanupExpirationBatchCount";
        }
    }
}
