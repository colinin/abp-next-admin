using LINGYUN.Abp.WeChat.Settings;

namespace LINGYUN.Abp.WeChat.MiniProgram.Settings
{
    public class WeChatMiniProgramSettingNames
    {
        private const string Prefix = WeChatSettingNames.Prefix + ".MiniProgram";

        public static string AppId = Prefix + "." + nameof(AbpWeChatMiniProgramOptions.AppId);
        public static string AppSecret = Prefix + "." + nameof(AbpWeChatMiniProgramOptions.AppSecret);
        public static string Token = Prefix + "." + nameof(AbpWeChatMiniProgramOptions.Token);
        public static string EncodingAESKey = Prefix + "." + nameof(AbpWeChatMiniProgramOptions.EncodingAESKey);
    }
}
