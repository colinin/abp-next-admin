using LINGYUN.Abp.WeChat.Work.ExternalContact.Features;
using LINGYUN.Abp.WeChat.Work.ExternalContact.MsgAudits.Request;
using LINGYUN.Abp.WeChat.Work.ExternalContact.MsgAudits.Response;
using LINGYUN.Abp.WeChat.Work.ExternalContact.Token;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.MsgAudits;

[RequiresFeature(WeChatWorkExternalContactFeatureNames.Enable)]
public class WeChatWorkMsgAuditProvider : IWeChatWorkMsgAuditProvider, ISingletonDependency
{
    protected IHttpClientFactory HttpClientFactory { get; }
    protected IWeChatWorkExternalContactTokenProvider WeChatWorkExternalContactTokenProvider { get; }

    public WeChatWorkMsgAuditProvider(
        IHttpClientFactory httpClientFactory,
        IWeChatWorkExternalContactTokenProvider weChatWorkExternalContactTokenProvider)
    {
        HttpClientFactory = httpClientFactory;
        WeChatWorkExternalContactTokenProvider = weChatWorkExternalContactTokenProvider;
    }

    public async virtual Task<WeChatWorkGetPermitUserListResponse> GetPermitUserListAsync(
        WeChatWorkGetPermitUserListRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkExternalContactTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.GetPermitUserListAsync(token.AccessToken, request, cancellationToken);
    }

    public async virtual Task<WeChatWorkCheckSingleAgreeResponse> CheckSingleAgreeAsync(
        WeChatWorkCheckSingleAgreeRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkExternalContactTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.CheckSingleAgreeAsync(token.AccessToken, request, cancellationToken);
    }

    public async virtual Task<WeChatWorkCheckRoomAgreeResponse> CheckRoomAgreeAsync(
        WeChatWorkCheckRoomAgreeRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkExternalContactTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.CheckRoomAgreeAsync(token.AccessToken, request, cancellationToken);
    }

    public async virtual Task<WeChatWorkGetGroupChatResponse> GetGroupChatAsync(
        WeChatWorkGetGroupChatRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkExternalContactTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.GetGroupChatAsync(token.AccessToken, request, cancellationToken);
    }
}
