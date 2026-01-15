using LINGYUN.Abp.WeChat.Work.OA.Approvals.Request;
using LINGYUN.Abp.WeChat.Work.OA.Approvals.Response;
using LINGYUN.Abp.WeChat.Work.OA.Features;
using LINGYUN.Abp.WeChat.Work.Token;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;

namespace LINGYUN.Abp.WeChat.Work.OA.Approvals;

[RequiresFeature(WeChatWorkOAFeatureNames.Enable)]
public class WeChatWorkApprovalTemplateProvider : IWeChatWorkApprovalTemplateProvider, ISingletonDependency
{
    protected IHttpClientFactory HttpClientFactory { get; }
    protected IWeChatWorkTokenProvider WeChatWorkTokenProvider { get; }

    public WeChatWorkApprovalTemplateProvider(
        IHttpClientFactory httpClientFactory,
        IWeChatWorkTokenProvider weChatWorkTokenProvider)
    {
        HttpClientFactory = httpClientFactory;
        WeChatWorkTokenProvider = weChatWorkTokenProvider;
    }

    public async virtual Task<WeChatWorkApplyEventResponse> ApplyEventAsync(WeChatWorkApplyEventRequest request, CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.ApplyEventAsync(token.AccessToken, request, cancellationToken);
    }

    public async virtual Task<WeChatWorkCreateTemplateResponse> CreateTemplateAsync(WeChatWorkCreateTemplateRequest request, CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.CreateTemplateAsync(token.AccessToken, request, cancellationToken);
    }

    public async virtual Task<WeChatWorkGetApprovalDetailResponse> GetApprovalDetailAsync(WeChatWorkGetApprovalDetailRequest request, CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.GetApprovalDetailAsync(token.AccessToken, request, cancellationToken);
    }

    public async virtual Task<WeChatWorkGetApprovalInfoResponse> GetApprovalInfoAsync(WeChatWorkGetApprovalInfoRequest request, CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.GetApprovalInfoAsync(token.AccessToken, request, cancellationToken);
    }

    public async virtual Task<WeChatWorkTemplateResponse> GetTemplateAsync(WeChatWorkGetTemplateRequest request, CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.GetTemplateAsync(token.AccessToken, request, cancellationToken);
    }

    public async virtual Task<WeChatWorkResponse> UpdateTemplateAsync(WeChatWorkCreateTemplateRequest request, CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.UpdateTemplateAsync(token.AccessToken, request, cancellationToken);
    }
}
