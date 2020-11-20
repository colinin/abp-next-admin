using LINGYUN.Abp.WeChat.MiniProgram.Settings;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Options;
using Volo.Abp.Settings;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.WeChat.MiniProgram
{
    public class AbpWeChatMiniProgramOptionsFactory : AbpOptionsFactory<AbpWeChatMiniProgramOptions>
    {
        protected ISettingProvider SettingProvider { get; }
        public AbpWeChatMiniProgramOptionsFactory(
            ISettingProvider settingProvider,
            IEnumerable<IConfigureOptions<AbpWeChatMiniProgramOptions>> setups, 
            IEnumerable<IPostConfigureOptions<AbpWeChatMiniProgramOptions>> postConfigures) 
            : base(setups, postConfigures)
        {
            SettingProvider = settingProvider;
        }

        public override AbpWeChatMiniProgramOptions Create(string name)
        {
            var options = base.Create(name);

            OverrideOptions(options);

            return options;
        }

        protected virtual void OverrideOptions(AbpWeChatMiniProgramOptions options)
        {
            AsyncHelper.RunSync(() => OverrideOptionsAsync(options));
        }

        protected virtual async Task OverrideOptionsAsync(AbpWeChatMiniProgramOptions options)
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
