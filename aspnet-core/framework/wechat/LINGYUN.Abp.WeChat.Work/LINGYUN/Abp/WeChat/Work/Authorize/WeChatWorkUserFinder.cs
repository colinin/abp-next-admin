using LINGYUN.Abp.WeChat.Work.Authorize.Models;
using LINGYUN.Abp.WeChat.Work.Authorize.Request;
using LINGYUN.Abp.WeChat.Work.Authorize.Response;
using LINGYUN.Abp.WeChat.Work.Features;
using LINGYUN.Abp.WeChat.Work.Token;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;

namespace LINGYUN.Abp.WeChat.Work.Authorize;

[RequiresFeature(WeChatWorkFeatureNames.Enable)]
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
        var userInfoResponse = await response.DeserializeObjectAsync<WeChatWorkUserInfoResponse>();

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
        var userDetailResponse = await response.DeserializeObjectAsync<WeChatWorkUserDetailResponse>();

        return userDetailResponse.ToUserDetail();
    }
}
