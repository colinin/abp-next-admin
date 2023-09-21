using LINGYUN.Abp.WeChat.Work.Authorize.Models;
using LINGYUN.Abp.WeChat.Work.Authorize.Request;
using LINGYUN.Abp.WeChat.Work.Authorize.Response;
using LINGYUN.Abp.WeChat.Work.Token;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.WeChat.Work.Authorize;
public class WeChatWorkUserFinder : IWeChatWorkUserFinder, ISingletonDependency
{
    protected IHttpClientFactory HttpClientFactory { get; }
    protected IWeChatWorkTokenProvider WeChatWorkTokenProvider { get; }

    public WeChatWorkUserFinder(
        IHttpClientFactory httpClientFactory,
        IWeChatWorkTokenProvider weChatWorkTokenProvider)
    {
        HttpClientFactory = httpClientFactory;
        WeChatWorkTokenProvider = weChatWorkTokenProvider;
    }

    public async virtual Task<WeChatWorkUserInfo> GetUserInfoAsync(
        string agentId,
        string code,
        CancellationToken cancellationToken = default)
    {
        var token = await WeChatWorkTokenProvider.GetTokenAsync(agentId, cancellationToken);
        var client = HttpClientFactory.CreateClient(AbpWeChatWorkGlobalConsts.ApiClient);

        using var response = await client.GetUserInfoAsync(token.AccessToken, code, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync();
        var userInfoResponse = JsonConvert.DeserializeObject<WeChatWorkUserInfoResponse>(responseContent);

        return userInfoResponse.ToUserInfo();
    }

    public async virtual Task<WeChatWorkUserDetail> GetUserDetailAsync(
        string agentId,
        string userTicket,
        CancellationToken cancellationToken = default)
    {
        var token = await WeChatWorkTokenProvider.GetTokenAsync(agentId, cancellationToken);
        var client = HttpClientFactory.CreateClient(AbpWeChatWorkGlobalConsts.ApiClient);

        var request = new WeChatWorkUserDetailRequest(userTicket);
        using var response = await client.GetUserDetailAsync(token.AccessToken, request, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync();
        var userDetailResponse = JsonConvert.DeserializeObject<WeChatWorkUserDetailResponse>(responseContent);

        return userDetailResponse.ToUserDetail();
    }
}
