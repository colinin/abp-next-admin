using LINGYUN.Abp.WeChat.Work.ExternalContact.Features;
using LINGYUN.Abp.WeChat.Work.ExternalContact.Transfers.Request;
using LINGYUN.Abp.WeChat.Work.ExternalContact.Transfers.Response;
using LINGYUN.Abp.WeChat.Work.Token;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Transfers;

[RequiresFeature(WeChatWorkExternalContactFeatureNames.Enable)]
public class WeChatWorkResignExtendProvider : IWeChatWorkResignExtendProvider, ISingletonDependency
{
    protected IHttpClientFactory HttpClientFactory { get; }
    protected IWeChatWorkTokenProvider WeChatWorkTokenProvider { get; }

    public WeChatWorkResignExtendProvider(
        IHttpClientFactory httpClientFactory,
        IWeChatWorkTokenProvider weChatWorkTokenProvider)
    {
        HttpClientFactory = httpClientFactory;
        WeChatWorkTokenProvider = weChatWorkTokenProvider;
    }

    public async virtual Task<WeChatWorkGetUnassignedListResponse> GetUnassignedListAsync(
        WeChatWorkGetUnassignedListRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        using var response = await client.GetUnassignedListAsync(token.AccessToken, request, cancellationToken);

        var wechatResponse = await response.DeserializeObjectAsync<WeChatWorkGetUnassignedListResponse>();
        wechatResponse.ThrowIfNotSuccess();
        return wechatResponse;
    }

    public async virtual Task<WeChatWorkResignedTransferCustomerResponse> ResignedTransferCustomerAsync(
        WeChatWorkResignedTransferCustomerRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        using var response = await client.ResignedTransferCustomerAsync(token.AccessToken, request, cancellationToken);

        var wechatResponse = await response.DeserializeObjectAsync<WeChatWorkResignedTransferCustomerResponse>();
        wechatResponse.ThrowIfNotSuccess();
        return wechatResponse;
    }

    public async virtual Task<WeChatWorkGetResignedTransferResultResponse> GetResignedTransferResultAsync(
        WeChatWorkGetResignedTransferResultRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        using var response = await client.GetResignedTransferResultAsync(token.AccessToken, request, cancellationToken);

        var wechatResponse = await response.DeserializeObjectAsync<WeChatWorkGetResignedTransferResultResponse>();
        wechatResponse.ThrowIfNotSuccess();
        return wechatResponse;
    }

    public async virtual Task<WeChatWorkGroupChatTransferResponse> GroupChatTransferAsync(
        WeChatWorkGroupChatTransferRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        using var response = await client.GroupChatTransferAsync(token.AccessToken, request, cancellationToken);

        var wechatResponse = await response.DeserializeObjectAsync<WeChatWorkGroupChatTransferResponse>();
        wechatResponse.ThrowIfNotSuccess();
        return wechatResponse;
    }
}
