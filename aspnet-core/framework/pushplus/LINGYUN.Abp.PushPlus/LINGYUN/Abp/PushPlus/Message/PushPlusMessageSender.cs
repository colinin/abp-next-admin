using LINGYUN.Abp.Features.LimitValidation;
using LINGYUN.Abp.PushPlus.Channel;
using LINGYUN.Abp.PushPlus.Features;
using LINGYUN.Abp.PushPlus.Settings;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;
using Volo.Abp.Json;
using Volo.Abp.Settings;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.PushPlus.Message;

[RequiresFeature(PushPlusFeatureNames.Message.Enable)]
public class PushPlusMessageSender : IPushPlusMessageSender, ITransientDependency
{
    protected ILogger<PushPlusMessageSender> Logger { get; }
    protected IClock Clock { get; }
    protected IJsonSerializer JsonSerializer { get; }
    protected ISettingProvider SettingProvider { get; }
    protected IHttpClientFactory HttpClientFactory { get; }

    public PushPlusMessageSender(
        ILogger<PushPlusMessageSender> logger,
        IClock clock,
        IJsonSerializer jsonSerializer,
        ISettingProvider settingProvider,
        IHttpClientFactory httpClientFactory)
    {
        Logger = logger;
        Clock = clock;
        JsonSerializer = jsonSerializer;
        SettingProvider = settingProvider;
        HttpClientFactory = httpClientFactory;
    }

    [RequiresFeature(PushPlusFeatureNames.Channel.WeChat.Enable)]
    [RequiresLimitFeature(
        PushPlusFeatureNames.Channel.WeChat.SendLimit,
        PushPlusFeatureNames.Channel.WeChat.SendLimitInterval, 
        LimitPolicy.Days)]
    public async virtual Task<string> SendWeChatAsync(
        string title,
        string content,
        string topic = "",
        PushPlusMessageTemplate template = PushPlusMessageTemplate.Html,
        string webhook = "",
        string callbackUrl = "",
        CancellationToken cancellationToken = default)
    {
        return await SendAsync(
            title,
            content,
            topic,
            template,
            PushPlusChannelType.WeChat,
            webhook,
            callbackUrl,
            cancellationToken);
    }

    [RequiresFeature(PushPlusFeatureNames.Channel.WeWork.Enable)]
    [RequiresLimitFeature(
        PushPlusFeatureNames.Channel.WeWork.SendLimit,
        PushPlusFeatureNames.Channel.WeWork.SendLimitInterval,
        LimitPolicy.Days)]
    public async virtual Task<string> SendWeWorkAsync(
        string title,
        string content,
        string topic = "",
        PushPlusMessageTemplate template = PushPlusMessageTemplate.Html,
        string webhook = "",
        string callbackUrl = "",
        CancellationToken cancellationToken = default)
    {
        return await SendAsync(
            title,
            content,
            topic,
            template,
            PushPlusChannelType.WeWork,
            webhook,
            callbackUrl,
            cancellationToken);
    }

    [RequiresFeature(PushPlusFeatureNames.Channel.Email.Enable)]
    [RequiresLimitFeature(
        PushPlusFeatureNames.Channel.Email.SendLimit,
        PushPlusFeatureNames.Channel.Email.SendLimitInterval,
        LimitPolicy.Days)]
    public async virtual Task<string> SendEmailAsync(
        string title,
        string content,
        string topic = "",
        PushPlusMessageTemplate template = PushPlusMessageTemplate.Html,
        string webhook = "",
        string callbackUrl = "",
        CancellationToken cancellationToken = default)
    {
        return await SendAsync(
            title,
            content,
            topic,
            template,
            PushPlusChannelType.Email,
            webhook,
            callbackUrl,
            cancellationToken);
    }

    [RequiresFeature(PushPlusFeatureNames.Channel.Webhook.Enable)]
    [RequiresLimitFeature(
        PushPlusFeatureNames.Channel.Webhook.SendLimit,
        PushPlusFeatureNames.Channel.Webhook.SendLimitInterval,
        LimitPolicy.Days)]
    public async virtual Task<string> SendWebhookAsync(
        string title,
        string content,
        string topic = "",
        PushPlusMessageTemplate template = PushPlusMessageTemplate.Html,
        string webhook = "",
        string callbackUrl = "",
        CancellationToken cancellationToken = default)
    {
        return await SendAsync(
            title,
            content,
            topic,
            template,
            PushPlusChannelType.Webhook,
            webhook,
            callbackUrl,
            cancellationToken);
    }

    [RequiresFeature(PushPlusFeatureNames.Channel.Sms.Enable)]
    [RequiresLimitFeature(
        PushPlusFeatureNames.Channel.Sms.SendLimit,
        PushPlusFeatureNames.Channel.Sms.SendLimitInterval,
        LimitPolicy.Days)]
    public async virtual Task<string> SendSmsAsync(
        string title,
        string content,
        string topic = "",
        PushPlusMessageTemplate template = PushPlusMessageTemplate.Html,
        string webhook = "",
        string callbackUrl = "",
        CancellationToken cancellationToken = default)
    {
        return await SendAsync(
           title,
           content,
           topic,
           template,
           PushPlusChannelType.Sms,
           webhook,
           callbackUrl,
           cancellationToken);
    }

    protected async virtual Task<string> SendAsync(
        string title, 
        string content,
        string topic = "",
        PushPlusMessageTemplate template = PushPlusMessageTemplate.Html,
        PushPlusChannelType channel = PushPlusChannelType.WeChat, 
        string webhook = "", 
        string callbackUrl = "",
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrEmpty(title, nameof(title));
        Check.NotNullOrEmpty(content, nameof(content));

        var token = await SettingProvider.GetOrNullAsync(PushPlusSettingNames.Security.Token);
        Check.NotNullOrEmpty(token, PushPlusSettingNames.Security.Token);

        var client = HttpClientFactory.GetPushPlusClient();

        var sendContent = await client.GetSendMessageContentAsync(
            token,
            title,
            content,
            topic,
            GetTemplate(template),
            channel.GetChannelName(),
            webhook,
            callbackUrl,
            cancellationToken: cancellationToken);

        var pushPlusResponse = JsonSerializer.Deserialize<PushPlusResponse<string>>(sendContent);

        return pushPlusResponse.GetData();
    }

    private static string GetTemplate(PushPlusMessageTemplate template)
    {
        return template switch
        {
            PushPlusMessageTemplate.Html => "html",
            PushPlusMessageTemplate.Text => "txt",
            PushPlusMessageTemplate.Json => "json",
            PushPlusMessageTemplate.Markdown => "markdown",
            PushPlusMessageTemplate.CloudMonitor => "cloudMonitor",
            PushPlusMessageTemplate.Jenkins => "jenkins",
            PushPlusMessageTemplate.Route => "route",
            _ => "html",
        };
    }
}
