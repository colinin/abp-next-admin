namespace LINGYUN.Abp.MessageService.Chat
{
    public static class ChatNotificationNames
    {
        public const string GroupName = "LINGYUN.Abp.IM.Chat";

        public static class UserFriend
        {
            public const string Default = GroupName + ".UserFriend";

            public const string NeedValidation = Default + ".NeedValidation";
        }
    }
}
