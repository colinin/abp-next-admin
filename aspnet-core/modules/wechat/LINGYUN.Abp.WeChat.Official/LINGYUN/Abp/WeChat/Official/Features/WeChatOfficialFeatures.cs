using LINGYUN.Abp.WeChat.Features;

namespace LINGYUN.Abp.WeChat.Official.Features
{
    public static class WeChatOfficialFeatures
    {
        public const string GroupName = WeChatFeatures.GroupName + ".Official";

        public const string Enable = GroupName + ".Enable";

        public const string EnableAuthorization = GroupName + ".EnableAuthorization";
    }
}
