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
public class WeChatWorkCropTagProvider : IWeChatWorkCropTagProvider, ISingletonDependency
{
    protected IHttpClientFactory HttpClientFactory { get; }
    protected IWeChatWorkTokenProvider WeChatWorkTokenProvider { get; }

    public WeChatWorkCropTagProvider(
        IHttpClientFactory httpClientFactory,
        IWeChatWorkTokenProvider weChatWorkTokenProvider)
    {
        HttpClientFactory = httpClientFactory;
        WeChatWorkTokenProvider = weChatWorkTokenProvider;
    }

    public async virtual Task<WeChatWorkGetCropTagListResponse> GetCropTagListAsync(
        WeChatWorkGetCropTagListRequest request, 
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.GetCropTagListAsync(token.AccessToken, request, cancellationToken);
    }

    public async virtual Task<WeChatWorkCreateCropTagResponse> CreateCropTagAsync(
        WeChatWorkCreateCropTagRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.CreateCropTagAsync(token.AccessToken, request, cancellationToken);
    }

    public async virtual Task<WeChatWorkResponse> UpdateCropTagAsync(
        WeChatWorkUpdateCropTagRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.UpdateCropTagAsync(token.AccessToken, request, cancellationToken);
    }

    public async virtual Task<WeChatWorkResponse> DeleteCropTagAsync(
        WeChatWorkDeleteCropTagRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.DeleteCropTagAsync(token.AccessToken, request, cancellationToken);
    }

    public async virtual Task<WeChatWorkResponse> MarkCropTagAsync(
        WeChatWorkMarkCropTagRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.MarkCropTagAsync(token.AccessToken, request, cancellationToken);
    }
}
