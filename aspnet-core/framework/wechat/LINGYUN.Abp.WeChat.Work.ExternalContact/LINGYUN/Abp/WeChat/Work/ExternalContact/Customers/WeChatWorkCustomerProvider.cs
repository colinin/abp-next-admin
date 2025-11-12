using LINGYUN.Abp.WeChat.Work.ExternalContact.Customers.Request;
using LINGYUN.Abp.WeChat.Work.ExternalContact.Customers.Response;
using LINGYUN.Abp.WeChat.Work.ExternalContact.Features;
using LINGYUN.Abp.WeChat.Work.Token;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Customers;

[RequiresFeature(WeChatWorkExternalContactFeatureNames.Enable)]
public class WeChatWorkCustomerProvider : IWeChatWorkCustomerProvider, ISingletonDependency
{
    protected IHttpClientFactory HttpClientFactory { get; }
    protected IWeChatWorkTokenProvider WeChatWorkTokenProvider { get; }

    public WeChatWorkCustomerProvider(
        IHttpClientFactory httpClientFactory,
        IWeChatWorkTokenProvider weChatWorkTokenProvider)
    {
        HttpClientFactory = httpClientFactory;
        WeChatWorkTokenProvider = weChatWorkTokenProvider;
    }

    public async virtual Task<WeChatWorkGetCustomerListResponse> GetCustomerListAsync(
        string userId, 
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(userId, nameof(userId));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        using var response = await client.GetCustomerListAsync(token.AccessToken, userId, cancellationToken);

        var wechatResponse = await response.DeserializeObjectAsync<WeChatWorkGetCustomerListResponse>();
        wechatResponse.ThrowIfNotSuccess();
        return wechatResponse;
    }

    public async virtual Task<WeChatWorkBulkGetCustomerResponse> BulkGetCustomerAsync(
        WeChatWorkBulkGetCustomerRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        using var response = await client.BulkGetCustomerAsync(token.AccessToken, request, cancellationToken);

        var wechatResponse = await response.DeserializeObjectAsync<WeChatWorkBulkGetCustomerResponse>();
        wechatResponse.ThrowIfNotSuccess();
        return wechatResponse;
    }

    public async virtual Task<WeChatWorkGetCustomerResponse> GetCustomerAsync(
        string externalUserid,
        string? cursor = null,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(externalUserid, nameof(externalUserid));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        using var response = await client.GetCustomerAsync(token.AccessToken, externalUserid, cursor, cancellationToken);

        var wechatResponse = await response.DeserializeObjectAsync<WeChatWorkGetCustomerResponse>();
        wechatResponse.ThrowIfNotSuccess();
        return wechatResponse;
    }

    public async virtual Task<WeChatWorkResponse> UpdateCustomerRemarkAsync(
        WeChatWorkUpdateCustomerRemarkRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        using var response = await client.UpdateCustomerRemarkAsync(token.AccessToken, request, cancellationToken);

        var wechatResponse = await response.DeserializeObjectAsync<WeChatWorkResponse>();
        wechatResponse.ThrowIfNotSuccess();
        return wechatResponse;
    }
}
