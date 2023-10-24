using LINGYUN.Abp.PushPlus.Token;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace LINGYUN.Abp.PushPlus.Channel.Webhook;

public class PushPlusWebhookProvider : IPushPlusWebhookProvider, ITransientDependency
{
    protected ILogger<PushPlusWebhookProvider> Logger { get; }
    protected IJsonSerializer JsonSerializer { get; }
    protected IHttpClientFactory HttpClientFactory { get; }
    protected IPushPlusTokenProvider PushPlusTokenProvider { get; }

    public PushPlusWebhookProvider(
        ILogger<PushPlusWebhookProvider> logger,
        IJsonSerializer jsonSerializer,
        IHttpClientFactory httpClientFactory,
        IPushPlusTokenProvider pushPlusTokenProvider)
    {
        Logger = logger;
        JsonSerializer = jsonSerializer;
        HttpClientFactory = httpClientFactory;
        PushPlusTokenProvider = pushPlusTokenProvider;
    }

    public async virtual Task<int> CreateWebhookAsync(
        string webhookCode, 
        string webhookName, 
        PushPlusWebhookType webhookType, 
        string webhookUrl,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(webhookCode, nameof(webhookCode));
        Check.NotNullOrWhiteSpace(webhookName, nameof(webhookName));
        Check.NotNullOrWhiteSpace(webhookUrl, nameof(webhookUrl));
        Check.NotNull(webhookType, nameof(webhookType));

        var token = await PushPlusTokenProvider.GetTokenAsync();
        var client = HttpClientFactory.GetPushPlusClient();

        var content = await client.GetCreateWebhookContentAsync(
            token.AccessKey,
            webhookCode,
            webhookName,
            webhookType,
            webhookUrl,
            cancellationToken);

        var pushPlusResponse = JsonSerializer.Deserialize<PushPlusResponse<int>>(content);

        return pushPlusResponse.GetData();
    }

    public async virtual Task<PushPlusWebhook> GetWebhookAsync(
        int webhookId, 
        CancellationToken cancellationToken = default)
    {
        var token = await PushPlusTokenProvider.GetTokenAsync();
        var client = HttpClientFactory.GetPushPlusClient();

        var content = await client.GetWebhookContentAsync(
            token.AccessKey,
            webhookId,
            cancellationToken);

        var pushPlusResponse = JsonSerializer.Deserialize<PushPlusResponse<PushPlusWebhook>>(content);

        return pushPlusResponse.GetData();
    }

    public async virtual Task<PushPlusPagedResponse<PushPlusWebhook>> GetWebhookListAsync(
        int current = 1, 
        int pageSize = 20, 
        CancellationToken cancellationToken = default)
    {
        var token = await PushPlusTokenProvider.GetTokenAsync();
        var client = HttpClientFactory.GetPushPlusClient();

        var content = await client.GetWebhookListContentAsync(
            token.AccessKey,
            current,
            pageSize,
            cancellationToken);

        var pushPlusResponse = JsonSerializer
            .Deserialize<PushPlusResponse<PushPlusPagedResponse<PushPlusWebhook>>>(content);

        return pushPlusResponse.GetData();
    }

    public async virtual Task<string> UpdateWebhookAsync(
        int id, 
        string webhookCode, 
        string webhookName, 
        PushPlusWebhookType webhookType, 
        string webhookUrl, 
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(webhookCode, nameof(webhookCode));
        Check.NotNullOrWhiteSpace(webhookName, nameof(webhookName));
        Check.NotNullOrWhiteSpace(webhookUrl, nameof(webhookUrl));
        Check.NotNull(webhookType, nameof(webhookType));

        var token = await PushPlusTokenProvider.GetTokenAsync();
        var client = HttpClientFactory.GetPushPlusClient();

        var content = await client.GetUpdateWebhookContentAsync(
            token.AccessKey,
            id,
            webhookCode,
            webhookName,
            webhookType,
            webhookUrl,
            cancellationToken);

        var pushPlusResponse = JsonSerializer.Deserialize<PushPlusResponse<string>>(content);

        return pushPlusResponse.GetData();
    }
}
