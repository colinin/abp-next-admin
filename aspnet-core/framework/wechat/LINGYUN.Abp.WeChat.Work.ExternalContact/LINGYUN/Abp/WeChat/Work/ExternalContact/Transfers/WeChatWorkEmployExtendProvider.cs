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
public class WeChatWorkEmployExtendProvider : IWeChatWorkEmployExtendProvider, ISingletonDependency
{
    protected IHttpClientFactory HttpClientFactory { get; }
    protected IWeChatWorkTokenProvider WeChatWorkTokenProvider { get; }

    public WeChatWorkEmployExtendProvider(
        IHttpClientFactory httpClientFactory,
        IWeChatWorkTokenProvider weChatWorkTokenProvider)
    {
        HttpClientFactory = httpClientFactory;
        WeChatWorkTokenProvider = weChatWorkTokenProvider;
    }

    public async virtual Task<WeChatWorkTransferCustomerResponse> TransferCustomerAsync(
        WeChatWorkTransferCustomerRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        using var response = await client.AssignCustomerAsync(token.AccessToken, request, cancellationToken);

        var wechatResponse = await response.DeserializeObjectAsync<WeChatWorkTransferCustomerResponse>();
        wechatResponse.ThrowIfNotSuccess();
        return wechatResponse;
    }

    public async virtual Task<WeChatWorkGetTransferResultResponse> GetTransferResultAsync(
        WeChatWorkGetTransferResultRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        using var response = await client.GetTransferResultAsync(token.AccessToken, request, cancellationToken);

        var wechatResponse = await response.DeserializeObjectAsync<WeChatWorkGetTransferResultResponse>();
        wechatResponse.ThrowIfNotSuccess();
        return wechatResponse;
    }

    public async virtual Task<WeChatWorkGroupChatOnjobTransferResponse> GroupChatOnjobTransferAsync(
        WeChatWorkGroupChatOnjobTransferRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        using var response = await client.OnjobTransferAsync(token.AccessToken, request, cancellationToken);

        var wechatResponse = await response.DeserializeObjectAsync<WeChatWorkGroupChatOnjobTransferResponse>();
        wechatResponse.ThrowIfNotSuccess();
        return wechatResponse;
    }
}
