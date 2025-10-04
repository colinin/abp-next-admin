using AspNet.Security.OAuth.Weixin;
using LINGYUN.Abp.WeChat.Official.Settings;
using System;
using System.Threading.Tasks;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Account.Web.OAuth.ExternalProviders.WeChat;

public class WeChatAuthHandlerOptionsProvider : OAuthHandlerOptionsProvider<WeixinAuthenticationOptions>
{
    public WeChatAuthHandlerOptionsProvider(ISettingProvider settingProvider) : base(settingProvider)
    {
    }

    public async override Task SetOptionsAsync(WeixinAuthenticationOptions options)
    {
        var clientId = await SettingProvider.GetOrNullAsync(WeChatOfficialSettingNames.AppId);
        var clientSecret = await SettingProvider.GetOrNullAsync(WeChatOfficialSettingNames.AppSecret);

        if (!clientId.IsNullOrWhiteSpace())
        {
            options.ClientId = clientId;
        }
        if (!clientSecret.IsNullOrWhiteSpace())
        {
            options.ClientSecret = clientSecret;
        }
        await base.SetOptionsAsync(options);
    }
}
