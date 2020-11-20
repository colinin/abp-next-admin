using LINGYUN.Abp.WeChat.Authorization.Settings;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Options;
using Volo.Abp.Settings;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.WeChat.Authorization
{
    public class AbpWeChatAuthorizationOptionsFactory : AbpOptionsFactory<AbpWeChatAuthorizationOptions>
    {
        protected ISettingProvider SettingProvider { get; }

        public AbpWeChatAuthorizationOptionsFactory(
            ISettingProvider settingProvider,
            IEnumerable<IConfigureOptions<AbpWeChatAuthorizationOptions>> setups, 
            IEnumerable<IPostConfigureOptions<AbpWeChatAuthorizationOptions>> postConfigures) 
            : base(setups, postConfigures)
        {
            SettingProvider = settingProvider;
        }

        public override AbpWeChatAuthorizationOptions Create(string name)
        {
            var options = base.Create(name);

            OverrideOptions(options);

            return options;
        }

        protected virtual void OverrideOptions(AbpWeChatAuthorizationOptions options)
        {
            AsyncHelper.RunSync(() => OverrideOptionsAsync(options));
        }

        protected virtual async Task OverrideOptionsAsync(AbpWeChatAuthorizationOptions options)
        {
            var appId = await SettingProvider.GetOrNullAsync(WeChatAuthorizationSettingNames.AppId);
            var appSecret = await SettingProvider.GetOrNullAsync(WeChatAuthorizationSettingNames.AppSecret);

            options.AppId = appId ?? options.AppId;
            options.AppSecret = appSecret ?? options.AppSecret;
        }
    }
}
