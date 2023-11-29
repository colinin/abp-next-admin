using LINGYUN.Abp.WeChat.Work.Messages.Request;
using LINGYUN.Abp.WeChat.Work.Token;
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

    public async virtual Task<bool> ReCallMessageAsync(string agentId, string messageId, CancellationToken cancellationToken = default)
    {
        var token = await WeChatWorkTokenProvider.GetTokenAsync(agentId, cancellationToken);
        var client = HttpClientFactory.CreateClient(AbpWeChatWorkGlobalConsts.ApiClient);

        var request = new WeChatWorkMessageReCallRequest(
            token.AccessToken,
            messageId);

        using var response = await client.ReCallMessageAsync(request, cancellationToken);
        var messageResponse = await response.DeserializeObjectAsync<WeChatWorkResponse>();

        return messageResponse.IsSuccessed;
    }
}
