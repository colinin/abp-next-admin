using LINGYUN.Abp.WeChat.Work.Chat.Request;
using LINGYUN.Abp.WeChat.Work.Chat.Response;
using LINGYUN.Abp.WeChat.Work.Features;
using LINGYUN.Abp.WeChat.Work.Token;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;

namespace LINGYUN.Abp.WeChat.Work.Chat;

[RequiresFeature(WeChatWorkFeatureNames.Enable)]
public class WeChatWorkAppChatManager : IWeChatWorkAppChatManager, ISingletonDependency
{
    protected IHttpClientFactory HttpClientFactory { get; }
    protected IWeChatWorkTokenProvider WeChatWorkTokenProvider { get; }

    public WeChatWorkAppChatManager(
        IHttpClientFactory httpClientFactory,
        IWeChatWorkTokenProvider weChatWorkTokenProvider)
    {
        HttpClientFactory = httpClientFactory;
        WeChatWorkTokenProvider = weChatWorkTokenProvider;
    }

    public async virtual Task<WeChatWorkAppChatCreateResponse> CreateAsync(
        WeChatWorkAppChatCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        var token = await WeChatWorkTokenProvider.GetTokenAsync(request.AgentId, cancellationToken);
        var client = HttpClientFactory.CreateClient(AbpWeChatWorkGlobalConsts.ApiClient);

        using var response = await client.CreateAppChatAsync(token.AccessToken, request, cancellationToken);
        return await response.DeserializeObjectAsync<WeChatWorkAppChatCreateResponse>();
    }

    public async virtual Task<WeChatWorkAppChatInfoResponse> GetAsync(
        string agentId,
        string chatId,
        CancellationToken cancellationToken = default)
    {
        var token = await WeChatWorkTokenProvider.GetTokenAsync(agentId, cancellationToken);
        var client = HttpClientFactory.CreateClient(AbpWeChatWorkGlobalConsts.ApiClient);

        using var response = await client.GetAppChatAsync(token.AccessToken, agentId, cancellationToken);
        return await response.DeserializeObjectAsync<WeChatWorkAppChatInfoResponse>();
    }

    public async virtual Task<WeChatWorkResponse> UpdateAsync(
        WeChatWorkAppChatUpdateRequest request, 
        CancellationToken cancellationToken = default)
    {
        var token = await WeChatWorkTokenProvider.GetTokenAsync(request.AgentId, cancellationToken);
        var client = HttpClientFactory.CreateClient(AbpWeChatWorkGlobalConsts.ApiClient);

        using var response = await client.UpdateAppChatAsync(token.AccessToken, request, cancellationToken);
        return await response.DeserializeObjectAsync<WeChatWorkAppChatCreateResponse>();
    }
}
