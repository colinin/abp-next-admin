using LINGYUN.Abp.WeChat.MiniProgram.Settings;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp.Options;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.WeChat.MiniProgram
{
    public class AbpWeChatMiniProgramOptionsManager : AbpDynamicOptionsManager<AbpWeChatMiniProgramOptions>
    {
        protected ISettingProvider SettingProvider { get; }

        public AbpWeChatMiniProgramOptionsManager(
            ISettingProvider settingProvider,
            IOptionsFactory<AbpWeChatMiniProgramOptions> factory) 
            : base(factory)
        {
            SettingProvider = settingProvider;
        }

        protected override async Task OverrideOptionsAsync(string name, AbpWeChatMiniProgramOptions options)
        {
            var appId = await SettingProvider.GetOrNullAsync(WeChatMiniProgramSettingNames.AppId);
            var appSecret = await SettingProvider.GetOrNullAsync(WeChatMiniProgramSettingNames.AppSecret);
            var token = await SettingProvider.GetOrNullAsync(WeChatMiniProgramSettingNames.Token);
            var aesKey = await SettingProvider.GetOrNullAsync(WeChatMiniProgramSettingNames.EncodingAESKey);

            options.AppId = appId ?? options.AppId;
            options.AppSecret = appSecret ?? options.AppSecret;
            options.Token = token ?? options.Token;
            options.EncodingAESKey = aesKey ?? options.EncodingAESKey;
        }
    }
}
