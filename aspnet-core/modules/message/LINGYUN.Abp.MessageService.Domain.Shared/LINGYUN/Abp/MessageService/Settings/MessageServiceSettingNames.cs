namespace LINGYUN.Abp.MessageService.Settings
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

        public class Messages
        {
            public const string Default = GroupName + ".Messages";
            /// <summary>
            /// 撤回消息过期时间（分）
            /// </summary>
            public const string RecallExpirationTime = Default + ".RecallExpirationTime";
        }
    }
}
