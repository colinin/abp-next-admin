using LINGYUN.Abp.Features.LimitValidation;
using LINGYUN.Abp.WeChat.Work.Features;
using LINGYUN.Abp.WeChat.Work.Token;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;

namespace LINGYUN.Abp.WeChat.Work.Message;

[RequiresFeature(WeChatWorkFeatureNames.Enable)]
public class WeChatWorkMessageSender : IWeChatWorkMessageSender, ISingletonDependency
{
    public ILogger<WeChatWorkMessageSender> Logger { get; set; }

    protected IHttpClientFactory HttpClientFactory { get; }
    protected IWeChatWorkTokenProvider WeChatWorkTokenProvider { get; }

    public WeChatWorkMessageSender(
        IHttpClientFactory httpClientFactory,
        IWeChatWorkTokenProvider weChatWorkTokenProvider)
    {
        HttpClientFactory = httpClientFactory;
        WeChatWorkTokenProvider = weChatWorkTokenProvider;

        Logger = NullLogger<WeChatWorkMessageSender>.Instance;
    }

    [RequiresFeature(WeChatWorkFeatureNames.Message.Enable)]
    [RequiresLimitFeature(
        WeChatWorkFeatureNames.Message.SendLimit,
        WeChatWorkFeatureNames.Message.SendLimitInterval,
        LimitPolicy.Days)]
    public async virtual Task<WeChatWorkMessageResponse> SendAsync(WeChatWorkMessage message, CancellationToken cancellationToken = default)
    {
        var token = await WeChatWorkTokenProvider.GetTokenAsync(message.AgentId, cancellationToken);
        var client = HttpClientFactory.CreateClient(AbpWeChatWorkGlobalConsts.ApiClient);

        var request = new WeChatWorkMessageRequest(
            token.AccessToken,
            message);

        using var response = await client.SendMessageAsync(request, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync();

        var messageResponse = JsonConvert.DeserializeObject<WeChatWorkMessageResponse>(responseContent);
        if (!messageResponse.IsSuccessed)
        {
            Logger.LogWarning("Send wechat work message failed");
            Logger.LogWarning($"Error code: {messageResponse.ErrorCode}, message: {messageResponse.ErrorMessage}");
        }

        return messageResponse;
    }
}
