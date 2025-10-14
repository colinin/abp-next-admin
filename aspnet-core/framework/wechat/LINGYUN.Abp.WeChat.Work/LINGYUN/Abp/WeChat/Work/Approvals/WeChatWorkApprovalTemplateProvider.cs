using LINGYUN.Abp.WeChat.Work.Approvals.Request;
using LINGYUN.Abp.WeChat.Work.Approvals.Response;
using LINGYUN.Abp.WeChat.Work.Features;
using LINGYUN.Abp.WeChat.Work.Token;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;

namespace LINGYUN.Abp.WeChat.Work.Approvals;

[RequiresFeature(WeChatWorkFeatureNames.Enable)]
public class WeChatWorkApprovalTemplateProvider : IApprovalTemplateProvider, ISingletonDependency
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
        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateClient(AbpWeChatWorkGlobalConsts.ApiClient);

        using var response = await client.ApplyEventAsync(token.AccessToken, request, cancellationToken);
        return await response.DeserializeObjectAsync<WeChatWorkApplyEventResponse>();
    }

    public async virtual Task<WeChatWorkCreateTemplateResponse> CreateTemplateAsync(WeChatWorkCreateTemplateRequest request, CancellationToken cancellationToken = default)
    {
        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateClient(AbpWeChatWorkGlobalConsts.ApiClient);

        using var response = await client.CreateTemplateAsync(token.AccessToken, request, cancellationToken);
        return await response.DeserializeObjectAsync<WeChatWorkCreateTemplateResponse>();
    }

    public async virtual Task<WeChatWorkGetApprovalDetailResponse> GetApprovalDetailAsync(WeChatWorkGetApprovalDetailRequest request, CancellationToken cancellationToken = default)
    {
        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateClient(AbpWeChatWorkGlobalConsts.ApiClient);

        using var response = await client.GetApprovalDetailAsync(token.AccessToken, request, cancellationToken);
        return await response.DeserializeObjectAsync<WeChatWorkGetApprovalDetailResponse>();
    }

    public async virtual Task<WeChatWorkGetApprovalInfoResponse> GetApprovalInfoAsync(WeChatWorkGetApprovalInfoRequest request, CancellationToken cancellationToken = default)
    {
        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateClient(AbpWeChatWorkGlobalConsts.ApiClient);

        using var response = await client.GetApprovalInfoAsync(token.AccessToken, request, cancellationToken);
        return await response.DeserializeObjectAsync<WeChatWorkGetApprovalInfoResponse>();
    }

    public async virtual Task<WeChatWorkTemplateResponse> GetTemplateAsync(WeChatWorkGetTemplateRequest request, CancellationToken cancellationToken = default)
    {
        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateClient(AbpWeChatWorkGlobalConsts.ApiClient);

        using var response = await client.GetTemplateAsync(token.AccessToken, request, cancellationToken);
        return await response.DeserializeObjectAsync<WeChatWorkTemplateResponse>();
    }

    public async virtual Task<WeChatWorkResponse> UpdateTemplateAsync(WeChatWorkCreateTemplateRequest request, CancellationToken cancellationToken = default)
    {
        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateClient(AbpWeChatWorkGlobalConsts.ApiClient);

        using var response = await client.UpdateTemplateAsync(token.AccessToken, request, cancellationToken);
        return await response.DeserializeObjectAsync<WeChatWorkResponse>();
    }
}
