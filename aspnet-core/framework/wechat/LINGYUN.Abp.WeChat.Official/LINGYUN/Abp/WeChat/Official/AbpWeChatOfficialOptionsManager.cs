using LINGYUN.Abp.WeChat.Official.Settings;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp.Options;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.WeChat.Official
{
    public class AbpWeChatOfficialOptionsManager : AbpDynamicOptionsManager<AbpWeChatOfficialOptions>
    {
        protected ISettingProvider SettingProvider { get; }
        public AbpWeChatOfficialOptionsManager(
            ISettingProvider settingProvider,
            IOptionsFactory<AbpWeChatOfficialOptions> factory)
            : base(factory)
        {
            SettingProvider = settingProvider;
        }

        protected override async Task OverrideOptionsAsync(string name, AbpWeChatOfficialOptions options)
        {
            var appId = await SettingProvider.GetOrNullAsync(WeChatOfficialSettingNames.AppId);
            var appSecret = await SettingProvider.GetOrNullAsync(WeChatOfficialSettingNames.AppSecret);
            var url = await SettingProvider.GetOrNullAsync(WeChatOfficialSettingNames.Url);
            var token = await SettingProvider.GetOrNullAsync(WeChatOfficialSettingNames.Token);
            var aesKey = await SettingProvider.GetOrNullAsync(WeChatOfficialSettingNames.EncodingAESKey);

            options.AppId = appId ?? options.AppId;
            options.AppSecret = appSecret ?? options.AppSecret;
            options.Url = url ?? options.Url;
            options.Token = token ?? options.Token;
            options.EncodingAESKey = aesKey ?? options.EncodingAESKey;
        }
    }
}
