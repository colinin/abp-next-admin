using LINGYUN.Abp.WeChat.Work.Features;
using LINGYUN.Abp.WeChat.Work.Tags.Request;
using LINGYUN.Abp.WeChat.Work.Tags.Response;
using LINGYUN.Abp.WeChat.Work.Token;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;

namespace LINGYUN.Abp.WeChat.Work.Tags;

[RequiresFeature(WeChatWorkFeatureNames.Enable)]
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
        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateClient(AbpWeChatWorkGlobalConsts.ApiClient);

        using var response = await client.AddTagMemberAsync(token.AccessToken, request, cancellationToken);
        return await response.DeserializeObjectAsync<WeChatWorkTagChangeMemberResponse>();
    }

    public async virtual Task<WeChatWorkTagCreateResponse> CreateAsync(WeChatWorkTagCreateRequest request, CancellationToken cancellationToken = default)
    {
        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateClient(AbpWeChatWorkGlobalConsts.ApiClient);

        using var response = await client.CreateTagAsync(token.AccessToken, request, cancellationToken);
        return await response.DeserializeObjectAsync<WeChatWorkTagCreateResponse>();
    }

    public async virtual Task<WeChatWorkResponse> DeleteAsync(WeChatWorkGetTagRequest request, CancellationToken cancellationToken = default)
    {
        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateClient(AbpWeChatWorkGlobalConsts.ApiClient);

        using var response = await client.DeleteTagAsync(token.AccessToken, request, cancellationToken);
        return await response.DeserializeObjectAsync<WeChatWorkResponse>();
    }

    public async virtual Task<WeChatWorkTagChangeMemberResponse> DeleteMemberAsync(WeChatWorkTagChangeMemberRequest request, CancellationToken cancellationToken = default)
    {
        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateClient(AbpWeChatWorkGlobalConsts.ApiClient);

        using var response = await client.DeleteTagMemberAsync(token.AccessToken, request, cancellationToken);
        return await response.DeserializeObjectAsync<WeChatWorkTagChangeMemberResponse>();
    }

    public async virtual Task<WeChatWorkTagListResponse> GetListAsync(CancellationToken cancellationToken = default)
    {
        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateClient(AbpWeChatWorkGlobalConsts.ApiClient);

        using var response = await client.GetTagListAsync(token.AccessToken, cancellationToken);
        return await response.DeserializeObjectAsync<WeChatWorkTagListResponse>();
    }

    public async virtual Task<WeChatWorkTagMemberInfoResponse> GetMemberAsync(WeChatWorkGetTagRequest request, CancellationToken cancellationToken = default)
    {
        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateClient(AbpWeChatWorkGlobalConsts.ApiClient);

        using var response = await client.GetTagMemberAsync(token.AccessToken, request, cancellationToken);
        return await response.DeserializeObjectAsync<WeChatWorkTagMemberInfoResponse>();
    }

    public async virtual Task<WeChatWorkResponse> UpdateAsync(WeChatWorkTagUpdateRequest request, CancellationToken cancellationToken = default)
    {
        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateClient(AbpWeChatWorkGlobalConsts.ApiClient);

        using var response = await client.UpdateTagAsync(token.AccessToken, request, cancellationToken);
        return await response.DeserializeObjectAsync<WeChatWorkResponse>();
    }
}
