using AspNet.Security.OAuth.QQ;
using LINGYUN.Abp.Tencent.QQ.Settings;
using System;
using System.Threading.Tasks;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Account.Web.OAuth.ExternalProviders.QQ;

public class QQAuthHandlerOptionsProvider : OAuthHandlerOptionsProvider<QQAuthenticationOptions>
{
    public QQAuthHandlerOptionsProvider(ISettingProvider settingProvider) : base(settingProvider)
    {
    }

    public async override Task SetOptionsAsync(QQAuthenticationOptions options)
    {
        var clientId = await SettingProvider.GetOrNullAsync(TencentQQSettingNames.QQConnect.AppId);
        var clientSecret = await SettingProvider.GetOrNullAsync(TencentQQSettingNames.QQConnect.AppKey);

        if (!clientId.IsNullOrWhiteSpace())
        {
            options.ClientId = clientId;
        }
        if (!clientSecret.IsNullOrWhiteSpace())
        {
            options.ClientSecret = clientSecret;
        }
    }
}
