using LINGYUN.Abp.WeChat.Work.ExternalContact.Features;
using LINGYUN.Abp.WeChat.Work.ExternalContact.GroupChats.Request;
using LINGYUN.Abp.WeChat.Work.ExternalContact.GroupChats.Response;
using LINGYUN.Abp.WeChat.Work.Token;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.GroupChats;

[RequiresFeature(WeChatWorkExternalContactFeatureNames.Enable)]
public class WeChatWorkGroupChatProvider : IWeChatWorkGroupChatProvider, ISingletonDependency
{
    protected IHttpClientFactory HttpClientFactory { get; }
    protected IWeChatWorkTokenProvider WeChatWorkTokenProvider { get; }

    public WeChatWorkGroupChatProvider(
        IHttpClientFactory httpClientFactory,
        IWeChatWorkTokenProvider weChatWorkTokenProvider)
    {
        HttpClientFactory = httpClientFactory;
        WeChatWorkTokenProvider = weChatWorkTokenProvider;
    }

    public async virtual Task<WeChatWorkGetGroupChatListResponse> GetGroupChatListAsync(
        WeChatWorkGetGroupChatListRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.GetGroupChatListAsync(token.AccessToken, request, cancellationToken);
    }

    public async virtual Task<WeChatWorkGetGroupChatResponse> GetGroupChatAsync(
        WeChatWorkGetGroupChatRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.GetGroupChatAsync(token.AccessToken, request, cancellationToken);
    }

    public async virtual Task<WeChatWorkOpengIdToChatIdResponse> OpengIdToChatIdAsync(
        WeChatWorkOpengIdToChatIdRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.OpengIdToChatIdAsync(token.AccessToken, request, cancellationToken);
    }
}
