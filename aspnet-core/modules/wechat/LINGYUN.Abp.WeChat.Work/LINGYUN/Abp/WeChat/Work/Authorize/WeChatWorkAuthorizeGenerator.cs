using LINGYUN.Abp.WeChat.Work.Features;
using LINGYUN.Abp.WeChat.Work.Settings;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.WeChat.Work.Authorize;

[RequiresFeature(WeChatWorkFeatureNames.Enable)]
public class WeChatWorkAuthorizeGenerator : IWeChatWorkAuthorizeGenerator, ISingletonDependency
{
    protected ISettingProvider SettingProvider { get; }
    protected IHttpClientFactory HttpClientFactory { get; }

    public WeChatWorkAuthorizeGenerator(
        ISettingProvider settingProvider,
        IHttpClientFactory httpClientFactory)
    {
        SettingProvider = settingProvider;
        HttpClientFactory = httpClientFactory;
    }

    public async virtual Task<string> GenerateOAuth2AuthorizeAsync(
        string agentid,
        string redirectUri,
        string state,
        string responseType = "code",
        string scope = "snsapi_base")
    {
        var corpId = await SettingProvider.GetOrNullAsync(WeChatWorkSettingNames.Connection.CorpId);

        Check.NotNullOrEmpty(corpId, nameof(corpId));

        var client = HttpClientFactory.CreateClient(AbpWeChatWorkGlobalConsts.OAuthClient);

        var generatedUrlBuilder = new StringBuilder();

        generatedUrlBuilder
            .Append(client.BaseAddress.AbsoluteUri.EnsureEndsWith('/'))
            .Append("connect/oauth2/authorize")
            .AppendFormat("?appid={0}", corpId)
            .AppendFormat("&redirect_uri={0}", HttpUtility.UrlEncode(redirectUri))
            .AppendFormat("&response_type={0}", responseType)
            .AppendFormat("&scope={0}", scope)
            .AppendFormat("&state={0}", state)
            .AppendFormat("&agentid={0}", agentid)
            .Append("#wechat_redirect");

        return generatedUrlBuilder.ToString();
    }

    public virtual Task<string> GenerateOAuth2LoginAsync(
        string appid,
        string redirectUri,
        string state,
        string loginType = "ServiceApp",
        string agentid = "",
        string lang = "zh")
    {
        var client = HttpClientFactory.CreateClient(AbpWeChatWorkGlobalConsts.LoginClient);

        var generatedUrlBuilder = new StringBuilder();

        generatedUrlBuilder
            .Append(client.BaseAddress.AbsoluteUri.EnsureEndsWith('/'))
            .Append("wwlogin/sso/login")
            .AppendFormat("?login_type={0}", loginType)
            .AppendFormat("&appid={0}", appid)
            .AppendFormat("&agentid={0}", agentid)
            .AppendFormat("&redirect_uri={0}", HttpUtility.UrlEncode(redirectUri))
            .AppendFormat("&state={0}", state)
            .AppendFormat("&lang={0}", lang);

        return Task.FromResult(generatedUrlBuilder.ToString());
    }
}
