namespace LINGYUN.Abp.MessageService.Chat
{
    public static class UserChatFriendConsts
    {
        public static int MaxRemarkNameLength { get; set; } = UserChatCardConsts.MaxUserNameLength;
        public static int MaxDescriptionLength { get; set; } = UserChatCardConsts.MaxDescriptionLength;
    }
}
