using Volo.Abp.Users;

namespace LINGYUN.Abp.MessageService.Chat
{
    public class UserChatCardConsts
    {
        public static int MaxUserNameLength { get; set; } = AbpUserConsts.MaxUserNameLength;
        public static int MaxSignLength { get; set; } = 30;
        public static int MaxNickNameLength { get; set; } = AbpUserConsts.MaxUserNameLength;
        public static int MaxDescriptionLength { get; set; } = 50;
        public static int MaxAvatarUrlLength { get; set; } = 512;
    }
}
