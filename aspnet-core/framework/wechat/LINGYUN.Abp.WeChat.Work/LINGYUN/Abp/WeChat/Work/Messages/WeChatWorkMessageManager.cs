using LINGYUN.Abp.WeChat.Work.Messages.Request;
using LINGYUN.Abp.WeChat.Work.Token;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.WeChat.Work.Messages;

public class WeChatWorkMessageManager : IWeChatWorkMessageManager, ISingletonDependency
{
    public ILogger<WeChatWorkMessageManager> Logger { get; set; }

    protected IHttpClientFactory HttpClientFactory { get; }
    protected IWeChatWorkTokenProvider WeChatWorkTokenProvider { get; }

    public WeChatWorkMessageManager(
        IHttpClientFactory httpClientFactory,
        IWeChatWorkTokenProvider weChatWorkTokenProvider)
    {
        HttpClientFactory = httpClientFactory;
        WeChatWorkTokenProvider = weChatWorkTokenProvider;

        Logger = NullLogger<WeChatWorkMessageManager>.Instance;
    }

    public async virtual Task<bool> ReCallMessageAsync(string messageId, CancellationToken cancellationToken = default)
    {
        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        var request = new WeChatWorkMessageReCallRequest(
            token.AccessToken,
            messageId);

        var messageResponse = await client.ReCallMessageAsync(request, cancellationToken);

        return messageResponse.IsSuccessed;
    }
}
