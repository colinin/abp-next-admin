using LINGYUN.Abp.WeChat.Official.Settings;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Options;
using Volo.Abp.Settings;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.WeChat.Official
{
    public class AbpWeChatOfficialOptionsFactory : AbpOptionsFactory<AbpWeChatOfficialOptions>
    {
        protected ISettingProvider SettingProvider { get; }
        public AbpWeChatOfficialOptionsFactory(
            ISettingProvider settingProvider,
            IEnumerable<IConfigureOptions<AbpWeChatOfficialOptions>> setups, 
            IEnumerable<IPostConfigureOptions<AbpWeChatOfficialOptions>> postConfigures) 
            : base(setups, postConfigures)
        {
            SettingProvider = settingProvider;
        }

        public override AbpWeChatOfficialOptions Create(string name)
        {
            var options = base.Create(name);

            OverrideOptions(options);

            return options;
        }

        protected virtual void OverrideOptions(AbpWeChatOfficialOptions options)
        {
            AsyncHelper.RunSync(() => OverrideOptionsAsync(options));
        }

        protected virtual async Task OverrideOptionsAsync(AbpWeChatOfficialOptions options)
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
