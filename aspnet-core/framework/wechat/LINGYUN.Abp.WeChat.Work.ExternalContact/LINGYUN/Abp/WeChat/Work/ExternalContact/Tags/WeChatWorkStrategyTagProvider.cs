using LINGYUN.Abp.WeChat.Work.ExternalContact.Features;
using LINGYUN.Abp.WeChat.Work.ExternalContact.Tags.Request;
using LINGYUN.Abp.WeChat.Work.ExternalContact.Tags.Response;
using LINGYUN.Abp.WeChat.Work.Token;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Tags;

[RequiresFeature(WeChatWorkExternalContactFeatureNames.Enable)]
public class WeChatWorkStrategyTagProvider : IWeChatWorkStrategyTagProvider, ISingletonDependency
{
    protected IHttpClientFactory HttpClientFactory { get; }
    protected IWeChatWorkTokenProvider WeChatWorkTokenProvider { get; }

    public WeChatWorkStrategyTagProvider(
        IHttpClientFactory httpClientFactory,
        IWeChatWorkTokenProvider weChatWorkTokenProvider)
    {
        HttpClientFactory = httpClientFactory;
        WeChatWorkTokenProvider = weChatWorkTokenProvider;
    }

    public async virtual Task<WeChatWorkGetStrategyTagListResponse> GetStrategyTagListAsync(
        WeChatWorkGetStrategyTagListRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        using var response = await client.GetStrategyTagListAsync(token.AccessToken, request, cancellationToken);

        var wechatResponse = await response.DeserializeObjectAsync<WeChatWorkGetStrategyTagListResponse>();
        wechatResponse.ThrowIfNotSuccess();
        return wechatResponse;
    }

    public async virtual Task<WeChatWorkCreateStrategyTagResponse> CreateStrategyTagAsync(
        WeChatWorkCreateStrategyTagRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        using var response = await client.CreateStrategyTagAsync(token.AccessToken, request, cancellationToken);

        var wechatResponse = await response.DeserializeObjectAsync<WeChatWorkCreateStrategyTagResponse>();
        wechatResponse.ThrowIfNotSuccess();
        return wechatResponse;
    }

    public async virtual Task<WeChatWorkResponse> UpdateStrategyTagAsync(
        WeChatWorkUpdateStrategyTagRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        using var response = await client.UpdateStrategyTagAsync(token.AccessToken, request, cancellationToken);

        var wechatResponse = await response.DeserializeObjectAsync<WeChatWorkResponse>();
        wechatResponse.ThrowIfNotSuccess();
        return wechatResponse;
    }

    public async virtual Task<WeChatWorkResponse> DeleteStrategyTagAsync(
        WeChatWorkDeleteStrategyTagRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        using var response = await client.DeleteStrategyTagAsync(token.AccessToken, request, cancellationToken);

        var wechatResponse = await response.DeserializeObjectAsync<WeChatWorkResponse>();
        wechatResponse.ThrowIfNotSuccess();
        return wechatResponse;
    }
}
