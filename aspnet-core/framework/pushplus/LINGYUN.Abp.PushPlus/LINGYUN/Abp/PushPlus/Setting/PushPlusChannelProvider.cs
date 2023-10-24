using LINGYUN.Abp.PushPlus.Channel;
using LINGYUN.Abp.PushPlus.Token;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace LINGYUN.Abp.PushPlus.Setting;

public class PushPlusChannelProvider : IPushPlusChannelProvider, ITransientDependency
{
    protected ILogger<PushPlusChannelProvider> Logger { get; }
    protected IJsonSerializer JsonSerializer { get; }
    protected IHttpClientFactory HttpClientFactory { get; }
    protected IPushPlusTokenProvider PushPlusTokenProvider { get; }

    public PushPlusChannelProvider(
        ILogger<PushPlusChannelProvider> logger,
        IJsonSerializer jsonSerializer,
        IHttpClientFactory httpClientFactory,
        IPushPlusTokenProvider pushPlusTokenProvider)
    {
        Logger = logger;
        JsonSerializer = jsonSerializer;
        HttpClientFactory = httpClientFactory;
        PushPlusTokenProvider = pushPlusTokenProvider;
    }

    public async virtual Task<PushPlusChannel> GetDefaultChannelAsync(CancellationToken cancellationToken = default)
    {
        var token = await PushPlusTokenProvider.GetTokenAsync();
        var client = HttpClientFactory.GetPushPlusClient();

        var content = await client.GetDefaultChannelContentAsync(
            token.AccessKey,
            cancellationToken);

        var pushPlusResponse = JsonSerializer
            .Deserialize<PushPlusResponse<PushPlusChannel>>(content);

        return pushPlusResponse.GetData();
    }

    public async virtual Task UpdateDefaultChannelAsync(
        PushPlusChannelType defaultChannel, 
        string defaultWebhook = "", 
        CancellationToken cancellationToken = default)
    {
        var token = await PushPlusTokenProvider.GetTokenAsync();
        var client = HttpClientFactory.GetPushPlusClient();

        var content = await client.GetUpdateDefaultChannelContentAsync(
            token.AccessKey,
            defaultChannel.GetChannelName(),
            defaultWebhook,
            cancellationToken);

        var pushPlusResponse = JsonSerializer
            .Deserialize<PushPlusResponse<object>>(content);

        pushPlusResponse.ThrowOfFailed();
    }

    public async virtual Task UpdateRecevieLimitAsync(
        PushPlusChannelRecevieLimit recevieLimit,
        CancellationToken cancellationToken = default)
    {
        var token = await PushPlusTokenProvider.GetTokenAsync();
        var client = HttpClientFactory.GetPushPlusClient();

        var content = await client.GetUpdateRecevieLimitContentAsync(
            token.AccessKey,
            recevieLimit,
            cancellationToken);

        var pushPlusResponse = JsonSerializer
            .Deserialize<PushPlusResponse<object>>(content);

        pushPlusResponse.ThrowOfFailed();
    }
}
