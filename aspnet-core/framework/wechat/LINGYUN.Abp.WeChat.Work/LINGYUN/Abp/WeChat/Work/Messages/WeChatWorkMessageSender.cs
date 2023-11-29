using LINGYUN.Abp.Features.LimitValidation;
using LINGYUN.Abp.WeChat.Work.Features;
using LINGYUN.Abp.WeChat.Work.Messages.Request;
using LINGYUN.Abp.WeChat.Work.Messages.Response;
using LINGYUN.Abp.WeChat.Work.Token;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;

namespace LINGYUN.Abp.WeChat.Work.Messages;

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
        WeChatWorkFeatureNames.Message.Limit,
        WeChatWorkFeatureNames.Message.LimitInterval,
        LimitPolicy.Days)]
    public async virtual Task<WeChatWorkMessageResponse> SendAsync(WeChatWorkMessage message, CancellationToken cancellationToken = default)
    {
        var token = await WeChatWorkTokenProvider.GetTokenAsync(message.AgentId, cancellationToken);
        var client = HttpClientFactory.CreateClient(AbpWeChatWorkGlobalConsts.ApiClient);

        var request = new WeChatWorkMessageRequest<WeChatWorkMessage>(
            token.AccessToken,
            message);

        using var response = await client.SendMessageAsync(request, cancellationToken);
        var messageResponse = await response.DeserializeObjectAsync<WeChatWorkMessageResponse>();

        if (!messageResponse.IsSuccessed)
        {
            Logger.LogWarning("Send wechat work message failed");
            Logger.LogWarning($"Error code: {messageResponse.ErrorCode}, message: {messageResponse.ErrorMessage}");
        }

        return messageResponse;
    }

    [RequiresFeature(WeChatWorkFeatureNames.AppChat.Message.Enable)]
    [RequiresLimitFeature(
        WeChatWorkFeatureNames.AppChat.Message.Limit,
        WeChatWorkFeatureNames.AppChat.Message.LimitInterval,
        LimitPolicy.Minute)]
    public async virtual Task<WeChatWorkResponse> SendAsync(WeChatWorkAppChatMessage message, CancellationToken cancellationToken = default)
    {
        var token = await WeChatWorkTokenProvider.GetTokenAsync(message.AgentId, cancellationToken);
        var client = HttpClientFactory.CreateClient(AbpWeChatWorkGlobalConsts.ApiClient);

        var request = new WeChatWorkMessageRequest<WeChatWorkAppChatMessage>(
            token.AccessToken,
            message);

        using var response = await client.SendMessageAsync(request, cancellationToken);
        var messageResponse = await response.DeserializeObjectAsync<WeChatWorkResponse>();

        if (!messageResponse.IsSuccessed)
        {
            Logger.LogWarning("Send wechat work message failed");
            Logger.LogWarning($"Error code: {messageResponse.ErrorCode}, message: {messageResponse.ErrorMessage}");
        }

        return messageResponse;
    }
}
