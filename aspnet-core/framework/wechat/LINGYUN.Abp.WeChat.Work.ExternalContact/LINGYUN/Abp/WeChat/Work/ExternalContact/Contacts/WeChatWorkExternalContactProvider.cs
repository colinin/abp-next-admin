using LINGYUN.Abp.WeChat.Work.ExternalContact.Contacts.Request;
using LINGYUN.Abp.WeChat.Work.ExternalContact.Contacts.Response;
using LINGYUN.Abp.WeChat.Work.ExternalContact.Features;
using LINGYUN.Abp.WeChat.Work.Token;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Contacts;

[RequiresFeature(WeChatWorkExternalContactFeatureNames.Enable)]
public class WeChatWorkExternalContactProvider : IWeChatWorkExternalContactProvider, ISingletonDependency
{
    protected IHttpClientFactory HttpClientFactory { get; }
    protected IWeChatWorkTokenProvider WeChatWorkTokenProvider { get; }

    public WeChatWorkExternalContactProvider(
        IHttpClientFactory httpClientFactory,
        IWeChatWorkTokenProvider weChatWorkTokenProvider)
    {
        HttpClientFactory = httpClientFactory;
        WeChatWorkTokenProvider = weChatWorkTokenProvider;
    }

    public async virtual Task<WeChatWorkGetExternalContactListResponse> GetExternalContactListAsync(
        WeChatWorkGetExternalContactListRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        using var response = await client.GetExternalContactListAsync(token.AccessToken, request, cancellationToken);

        var wechatResponse = await response.DeserializeObjectAsync<WeChatWorkGetExternalContactListResponse>();
        wechatResponse.ThrowIfNotSuccess();
        return wechatResponse;
    }
}
