using LINGYUN.Abp.WeChat.Settings;

namespace LINGYUN.Abp.WeChat.Authorization.Settings
{
    public class WeChatAuthorizationSettingNames
    {
        private const string Prefix = WeChatSettingNames.Prefix + ".Authorization";
        public static string AppId = Prefix + "." + nameof(AbpWeChatAuthorizationOptions.AppId);
        public static string AppSecret = Prefix + "." + nameof(AbpWeChatAuthorizationOptions.AppSecret);
    }
}
