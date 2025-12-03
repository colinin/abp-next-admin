using LINGYUN.Abp.WeChat.Work.Contacts.Features;
using LINGYUN.Abp.WeChat.Work.Contacts.Tags.Request;
using LINGYUN.Abp.WeChat.Work.Contacts.Tags.Response;
using LINGYUN.Abp.WeChat.Work.Token;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Tags;

[RequiresFeature(WeChatWorkContactsFeatureNames.Enable)]
public class WeChatWorkTagProvider : IWeChatWorkTagProvider, ISingletonDependency
{
    protected IHttpClientFactory HttpClientFactory { get; }
    protected IWeChatWorkTokenProvider WeChatWorkTokenProvider { get; }

    public WeChatWorkTagProvider(
        IHttpClientFactory httpClientFactory,
        IWeChatWorkTokenProvider weChatWorkTokenProvider)
    {
        HttpClientFactory = httpClientFactory;
        WeChatWorkTokenProvider = weChatWorkTokenProvider;
    }

    public async virtual Task<WeChatWorkTagChangeMemberResponse> AddMemberAsync(WeChatWorkTagChangeMemberRequest request, CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.AddTagMemberAsync(token.AccessToken, request, cancellationToken);
    }

    public async virtual Task<WeChatWorkTagCreateResponse> CreateAsync(WeChatWorkTagCreateRequest request, CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.CreateTagAsync(token.AccessToken, request, cancellationToken);
    }

    public async virtual Task<WeChatWorkResponse> DeleteAsync(WeChatWorkGetTagRequest request, CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.DeleteTagAsync(token.AccessToken, request, cancellationToken);
    }

    public async virtual Task<WeChatWorkTagChangeMemberResponse> DeleteMemberAsync(WeChatWorkTagChangeMemberRequest request, CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.DeleteTagMemberAsync(token.AccessToken, request, cancellationToken);
    }

    public async virtual Task<WeChatWorkTagListResponse> GetListAsync(CancellationToken cancellationToken = default)
    {
        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.GetTagListAsync(token.AccessToken, cancellationToken);
    }

    public async virtual Task<WeChatWorkTagMemberInfoResponse> GetMemberAsync(WeChatWorkGetTagRequest request, CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.GetTagMemberAsync(token.AccessToken, request, cancellationToken);
    }

    public async virtual Task<WeChatWorkResponse> UpdateAsync(WeChatWorkTagUpdateRequest request, CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.UpdateTagAsync(token.AccessToken, request, cancellationToken);
    }
}
