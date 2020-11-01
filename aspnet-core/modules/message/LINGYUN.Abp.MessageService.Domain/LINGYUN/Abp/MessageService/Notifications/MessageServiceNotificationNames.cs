namespace LINGYUN.Abp.MessageService.Notifications
{
    public static class MessageServiceNotificationNames
    {
        public const string GroupName = "LINGYUN.Abp.Messages";

        public class IM
        {
            public const string GroupName = MessageServiceNotificationNames.GroupName + ".IM";
            /// <summary>
            /// 好友验证通知
            /// </summary>
            public const string FriendValidation = GroupName + ".FriendValidation";
            /// <summary>
            /// 新好友通知
            /// </summary>
            public const string NewFriend = GroupName + ".NewFriend";
            /// <summary>
            /// 加入群组通知
            /// </summary>
            public const string JoinGroup = GroupName + ".JoinGroup";
            /// <summary>
            /// 退出群组通知
            /// </summary>
            public const string ExitGroup = GroupName + ".ExitGroup";
            /// <summary>
            /// 群组解散通知
            /// </summary>
            public const string DissolveGroup = GroupName + ".DissolveGroup";
        }

    }
}
