using LINGYUN.Abp.PushPlus.Token;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Json;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.PushPlus.User;

public class PushPlusUserProvider : IPushPlusUserProvider, ITransientDependency
{
    protected ILogger<PushPlusUserProvider> Logger { get; }
    protected IJsonSerializer JsonSerializer { get; }
    protected IHttpClientFactory HttpClientFactory { get; }
    protected IPushPlusTokenProvider PushPlusTokenProvider { get; }

    public PushPlusUserProvider(
        ILogger<PushPlusUserProvider> logger,
        IJsonSerializer jsonSerializer,
        IHttpClientFactory httpClientFactory,
        IPushPlusTokenProvider pushPlusTokenProvider)
    {
        Logger = logger;
        JsonSerializer = jsonSerializer;
        HttpClientFactory = httpClientFactory;
        PushPlusTokenProvider = pushPlusTokenProvider;
    }

    public async virtual Task<PushPlusUserLimitTime> GetMyLimitTimeAsync(CancellationToken cancellationToken = default)
    {
        var token = await PushPlusTokenProvider.GetTokenAsync();
        var client = HttpClientFactory.GetPushPlusClient();

        var content = await client.GetLimitTimeContentAsync(
            token.AccessKey,
            cancellationToken);

        var pushPlusResponse = JsonSerializer
            .Deserialize<PushPlusResponse<PushPlusUserLimitTime>>(content);

        return pushPlusResponse.GetData();
    }

    public async virtual Task<PushPlusUserProfile> GetMyProfileAsync(CancellationToken cancellationToken = default)
    {
        var token = await PushPlusTokenProvider.GetTokenAsync();
        var client = HttpClientFactory.GetPushPlusClient();

        var content = await client.GetProfileContentAsync(
            token.AccessKey,
            cancellationToken);

        var pushPlusResponse = JsonSerializer
            .Deserialize<PushPlusResponse<PushPlusUserProfile>>(content);

        return pushPlusResponse.GetData();
    }

    public async virtual Task<string> GetMyTokenAsync(CancellationToken cancellationToken = default)
    {
        var token = await PushPlusTokenProvider.GetTokenAsync();
        var client = HttpClientFactory.GetPushPlusClient();

        var content = await client.GetTokenContentAsync(
            token.AccessKey,
            cancellationToken);

        var pushPlusResponse = JsonSerializer
            .Deserialize<PushPlusResponse<string>>(content);

        return pushPlusResponse.GetData();
    }
}
