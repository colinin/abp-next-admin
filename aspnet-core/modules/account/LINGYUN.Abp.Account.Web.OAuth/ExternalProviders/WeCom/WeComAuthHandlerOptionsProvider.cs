using AspNet.Security.OAuth.WorkWeixin;
using LINGYUN.Abp.WeChat.Work.Settings;
using System;
using System.Threading.Tasks;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Account.Web.OAuth.ExternalProviders.WeCom;

public class WeComAuthHandlerOptionsProvider : OAuthHandlerOptionsProvider<WorkWeixinAuthenticationOptions>
{
    public WeComAuthHandlerOptionsProvider(ISettingProvider settingProvider) : base(settingProvider)
    {
    }

    public async override Task SetOptionsAsync(WorkWeixinAuthenticationOptions options)
    {
        var clientId = await SettingProvider.GetOrNullAsync(WeChatWorkSettingNames.Connection.CorpId);
        var clientSecret = await SettingProvider.GetOrNullAsync(WeChatWorkSettingNames.Connection.Secret);
        var agentId = await SettingProvider.GetOrNullAsync(WeChatWorkSettingNames.Connection.AgentId);

        if (!clientId.IsNullOrWhiteSpace())
        {
            options.ClientId = clientId;
        }
        if (!clientSecret.IsNullOrWhiteSpace())
        {
            options.ClientSecret = clientSecret;
        }
        if (!agentId.IsNullOrWhiteSpace())
        {
            options.AgentId = agentId;
        }
        await base.SetOptionsAsync(options);
    }
}
