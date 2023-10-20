using LINGYUN.Abp.PushPlus.Token;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace LINGYUN.Abp.PushPlus.Message;

public class PushPlusMessageProvider : IPushPlusMessageProvider, ITransientDependency
{
    protected ILogger<PushPlusMessageProvider> Logger { get; }
    protected IJsonSerializer JsonSerializer { get; }
    protected IHttpClientFactory HttpClientFactory { get; }
    protected IPushPlusTokenProvider PushPlusTokenProvider { get; }

    public PushPlusMessageProvider(
        ILogger<PushPlusMessageProvider> logger, 
        IJsonSerializer jsonSerializer, 
        IHttpClientFactory httpClientFactory,
        IPushPlusTokenProvider pushPlusTokenProvider)
    {
        Logger = logger;
        JsonSerializer = jsonSerializer;
        HttpClientFactory = httpClientFactory;
        PushPlusTokenProvider = pushPlusTokenProvider;
    }

    public async virtual Task<PushPlusPagedResponse<PushPlusMessage>> GetMessageListAsync(
        int current, 
        int pageSize, 
        CancellationToken cancellationToken = default)
    {
        var token = await PushPlusTokenProvider.GetTokenAsync();
        var client = HttpClientFactory.GetPushPlusClient();

        var content = await client.GetMessageListContentAsync(
            token.AccessKey, 
            current,
            pageSize, 
            cancellationToken);

        var pushPlusResponse = JsonSerializer
            .Deserialize<PushPlusResponse<PushPlusPagedResponse<PushPlusMessage>>>(content);

        return pushPlusResponse.GetData();
    }

    public async virtual Task<SendPushPlusMessageResult> GetSendResultAsync(
        string shortCode, 
        CancellationToken cancellationToken = default)
    {
        var token = await PushPlusTokenProvider.GetTokenAsync();
        var client = HttpClientFactory.GetPushPlusClient();

        var content = await client.GetSendResultContentAsync(
            token.AccessKey,
            shortCode,
            cancellationToken);

        var pushPlusResponse = JsonSerializer
            .Deserialize<PushPlusResponse<SendPushPlusMessageResult>>(content);

        return pushPlusResponse.GetData();
    }
}
