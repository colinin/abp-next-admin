using LINGYUN.Abp.WeChat.Settings;

namespace LINGYUN.Abp.WeChat.Official.Settings
{
    public class WeChatOfficialSettingNames
    {
        private const string Prefix = WeChatSettingNames.Prefix + ".Official";

        public static string IsSandBox = Prefix + "." + nameof(AbpWeChatOfficialOptions.IsSandBox);
        public static string AppId = Prefix + "." + nameof(AbpWeChatOfficialOptions.AppId);
        public static string AppSecret = Prefix + "." + nameof(AbpWeChatOfficialOptions.AppSecret);
        public static string Url = Prefix + "." + nameof(AbpWeChatOfficialOptions.Url);
        public static string Token = Prefix + "." + nameof(AbpWeChatOfficialOptions.Token);
        public static string EncodingAESKey = Prefix + "." + nameof(AbpWeChatOfficialOptions.EncodingAESKey);
    }
}
