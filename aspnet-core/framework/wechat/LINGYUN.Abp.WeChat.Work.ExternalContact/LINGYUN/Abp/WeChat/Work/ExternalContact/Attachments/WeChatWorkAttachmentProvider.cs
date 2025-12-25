using LINGYUN.Abp.WeChat.Work.ExternalContact.Attachments.Request;
using LINGYUN.Abp.WeChat.Work.ExternalContact.Attachments.Response;
using LINGYUN.Abp.WeChat.Work.ExternalContact.Features;
using LINGYUN.Abp.WeChat.Work.Token;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Attachments;

[RequiresFeature(WeChatWorkExternalContactFeatureNames.Enable)]
public class WeChatWorkAttachmentProvider : IWeChatWorkAttachmentProvider, ISingletonDependency
{
    protected IHttpClientFactory HttpClientFactory { get; }
    protected IWeChatWorkTokenProvider WeChatWorkTokenProvider { get; }

    public WeChatWorkAttachmentProvider(
        IHttpClientFactory httpClientFactory,
        IWeChatWorkTokenProvider weChatWorkTokenProvider)
    {
        HttpClientFactory = httpClientFactory;
        WeChatWorkTokenProvider = weChatWorkTokenProvider;
    }

    public async virtual Task<WeChatWorkUploadAttachmentResponse> UploadAsync(
        WeChatWorkUploadAttachmentRequest request,
        CancellationToken cancellationToken = default)
    {
        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.UploadAsync(token.AccessToken, request, cancellationToken);
    }
}
