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
        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
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
        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateClient(AbpWeChatWorkGlobalConsts.ApiClient);

        var request = new WeChatWorkMessageRequest<WeChatWorkAppChatMessage>(
            token.AccessToken,
            message);

        using var response = await client.SendMessageAsync(request, cancellationToken);
        var messageResponse = await response.DeserializeObjectAsync<WeChatWorkResponse>();

        if (!messageResponse.IsSuccessed)
        {
            Logger.LogWarning("Send wechat work app chat message failed");
            Logger.LogWarning($"Error code: {messageResponse.ErrorCode}, message: {messageResponse.ErrorMessage}");
        }

        return messageResponse;
    }

    [RequiresFeature(WeChatWorkFeatureNames.Webhook.Enable)]
    [RequiresLimitFeature(
        WeChatWorkFeatureNames.Webhook.Limit,
        WeChatWorkFeatureNames.Webhook.LimitInterval,
        LimitPolicy.Minute)]
    // 消息发送频率限制: https://developer.work.weixin.qq.com/document/path/99110#%E6%B6%88%E6%81%AF%E5%8F%91%E9%80%81%E9%A2%91%E7%8E%87%E9%99%90%E5%88%B6
    public async virtual Task<WeChatWorkResponse> SendAsync(string webhookKey, WeChatWorkWebhookMessage message, CancellationToken cancellationToken = default)
    {
        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateClient(AbpWeChatWorkGlobalConsts.ApiClient);

        using var response = await client.SendMessageAsync(webhookKey, message, cancellationToken);
        var messageResponse = await response.DeserializeObjectAsync<WeChatWorkResponse>();

        if (!messageResponse.IsSuccessed)
        {
            Logger.LogWarning("Send wechat work webhook message failed");
            Logger.LogWarning($"Error code: {messageResponse.ErrorCode}, message: {messageResponse.ErrorMessage}");
        }

        return messageResponse;
    }
}
