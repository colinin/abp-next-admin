using LINGYUN.Abp.PushPlus.Token;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace LINGYUN.Abp.PushPlus.Topic;

public class PushPlusTopicProvider : IPushPlusTopicProvider, ITransientDependency
{
    protected ILogger<PushPlusTopicProvider> Logger { get; }
    protected IJsonSerializer JsonSerializer { get; }
    protected IHttpClientFactory HttpClientFactory { get; }
    protected IPushPlusTokenProvider PushPlusTokenProvider { get; }

    public PushPlusTopicProvider(
        ILogger<PushPlusTopicProvider> logger,
        IJsonSerializer jsonSerializer,
        IHttpClientFactory httpClientFactory,
        IPushPlusTokenProvider pushPlusTokenProvider)
    {
        Logger = logger;
        JsonSerializer = jsonSerializer;
        HttpClientFactory = httpClientFactory;
        PushPlusTokenProvider = pushPlusTokenProvider;
    }

    public async virtual Task<int> CreateTopicAsync(
        string topicCode, 
        string topicName, 
        string contact, 
        string introduction,
        string receiptMessage = "", 
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(topicCode, nameof(topicCode));
        Check.NotNullOrWhiteSpace(topicName, nameof(topicName));
        Check.NotNullOrWhiteSpace(contact, nameof(contact));
        Check.NotNullOrWhiteSpace(introduction, nameof(introduction));

        var token = await PushPlusTokenProvider.GetTokenAsync();
        var client = HttpClientFactory.GetPushPlusClient();

        var content = await client.GetCreateTopicContentAsync(
            token.AccessKey,
            topicCode,
            topicName,
            contact,
            introduction,
            receiptMessage,
            cancellationToken);

        var pushPlusResponse = JsonSerializer.Deserialize<PushPlusResponse<int>>(content);

        return pushPlusResponse.GetData();
    }

    public async virtual Task<PushPlusTopicForMe> GetTopicForMeProfileAsync(
        int topicId, 
        CancellationToken cancellationToken = default)
    {
        var token = await PushPlusTokenProvider.GetTokenAsync();
        var client = HttpClientFactory.GetPushPlusClient();

        var content = await client.GetTopicForMeProfileContentAsync(
            token.AccessKey,
            topicId,
            cancellationToken);

        var pushPlusResponse = JsonSerializer
            .Deserialize<PushPlusResponse<PushPlusTopicForMe>>(content);

        return pushPlusResponse.GetData();
    }

    public async virtual Task<PushPlusPagedResponse<PushPlusTopic>> GetTopicListAsync(
        int current, 
        int pageSize, 
        PushPlusTopicType topicType = PushPlusTopicType.Create, 
        CancellationToken cancellationToken = default)
    {
        var token = await PushPlusTokenProvider.GetTokenAsync();
        var client = HttpClientFactory.GetPushPlusClient();

        var content = await client.GetTopicListContentAsync(
            token.AccessKey,
            current,
            pageSize,
            topicType,
            cancellationToken);

        var pushPlusResponse = JsonSerializer
            .Deserialize<PushPlusResponse<PushPlusPagedResponse<PushPlusTopic>>>(content);

        return pushPlusResponse.GetData();
    }

    public async virtual Task<PushPlusTopicProfile> GetTopicProfileAsync(
        int topicId, 
        CancellationToken cancellationToken = default)
    {
        var token = await PushPlusTokenProvider.GetTokenAsync();
        var client = HttpClientFactory.GetPushPlusClient();

        var content = await client.GetTopicProfileContentAsync(
            token.AccessKey,
            topicId,
            cancellationToken);

        var pushPlusResponse = JsonSerializer
            .Deserialize<PushPlusResponse<PushPlusTopicProfile>>(content);

        return pushPlusResponse.GetData();
    }

    public async virtual Task<PushPlusTopicQrCode> GetTopicQrCodeAsync(
        int topicId,
        PushPlusTopicQrCodeType forever = PushPlusTopicQrCodeType.Temporary,
        CancellationToken cancellationToken = default)
    {
        var token = await PushPlusTokenProvider.GetTokenAsync();
        var client = HttpClientFactory.GetPushPlusClient();

        var content = await client.GetTopicQrCodeContentAsync(
            token.AccessKey,
            topicId,
            forever,
            cancellationToken);

        var pushPlusResponse = JsonSerializer
            .Deserialize<PushPlusResponse<PushPlusTopicQrCode>>(content);

        return pushPlusResponse.GetData();
    }

    public async virtual Task<string> QuitTopicAsync(
        int topicId, 
        CancellationToken cancellationToken = default)
    {
        var token = await PushPlusTokenProvider.GetTokenAsync();
        var client = HttpClientFactory.GetPushPlusClient();

        var content = await client.GetQuitTopicContentAsync(
            token.AccessKey,
            topicId,
            cancellationToken);

        var pushPlusResponse = JsonSerializer
            .Deserialize<PushPlusResponse<string>>(content);

        return pushPlusResponse.GetData();
    }

    public async virtual Task<PushPlusPagedResponse<PushPlusTopicUser>> GetSubscriberListAsync(
        int current,
        int pageSize,
        int topicId,
        CancellationToken cancellationToken = default)
    {
        var token = await PushPlusTokenProvider.GetTokenAsync();
        var client = HttpClientFactory.GetPushPlusClient();

        var content = await client.GetSubscriberListContentAsync(
            token.AccessKey,
            current,
            pageSize,
            topicId,
            cancellationToken);

        var pushPlusResponse = JsonSerializer
            .Deserialize<PushPlusResponse<PushPlusPagedResponse<PushPlusTopicUser>>>(content);

        return pushPlusResponse.GetData();
    }

    public async virtual Task<string> UnSubscriberAsync(
        int topicRelationId,
        CancellationToken cancellationToken = default)
    {
        var token = await PushPlusTokenProvider.GetTokenAsync();
        var client = HttpClientFactory.GetPushPlusClient();

        var content = await client.GetUnSubscriberContentAsync(
            token.AccessKey,
            topicRelationId,
            cancellationToken);

        var pushPlusResponse = JsonSerializer
            .Deserialize<PushPlusResponse<string>>(content);

        return pushPlusResponse.GetData();
    }
}
