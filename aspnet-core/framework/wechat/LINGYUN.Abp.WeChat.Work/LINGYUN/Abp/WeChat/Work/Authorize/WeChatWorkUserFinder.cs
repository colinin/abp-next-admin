using LINGYUN.Abp.WeChat.Work.Authorize.Models;
using LINGYUN.Abp.WeChat.Work.Authorize.Request;
using LINGYUN.Abp.WeChat.Work.Authorize.Response;
using LINGYUN.Abp.WeChat.Work.Features;
using LINGYUN.Abp.WeChat.Work.Token;
using Microsoft.Extensions.DependencyInjection;
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
        string code,
        CancellationToken cancellationToken = default)
    {
        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        var wechatWorkResponse = await client.GetUserInfoAsync(token.AccessToken, code, cancellationToken);

        return wechatWorkResponse.ToUserInfo();
    }

    public async virtual Task<WeChatWorkUserDetail> GetUserDetailAsync(
        string userTicket,
        CancellationToken cancellationToken = default)
    {
        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        var request = new WeChatWorkUserDetailRequest(userTicket);
        var wechatWorkResponse = await client.GetUserDetailAsync(token.AccessToken, request, cancellationToken);

        return wechatWorkResponse.ToUserDetail();
    }
}
