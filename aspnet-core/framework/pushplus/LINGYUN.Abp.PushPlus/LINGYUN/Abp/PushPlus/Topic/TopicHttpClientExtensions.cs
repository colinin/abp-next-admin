using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.PushPlus.Topic;

internal static class TopicHttpClientExtensions
{
    private const string _getTopicListTemplate = "{\"current\":$current,\"pageSize\":$pageSize,\"params\":{\"topicType\":$topicType}}";
    public async static Task<string> GetTopicListContentAsync(
        this HttpClient httpClient,
        string accessKey,
        int current,
        int pageSize,
        PushPlusTopicType topicType = PushPlusTopicType.Create,
        CancellationToken cancellationToken = default)
    {
        var requestMessage = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/open/topic/list");

        var requestBody = _getTopicListTemplate
            .Replace("$current", current.ToString())
            .Replace("$pageSize", pageSize.ToString())
            .Replace("$topicType", ((int)topicType).ToString());

        requestMessage.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
        requestMessage.Headers.TryAddWithoutValidation("access-key", accessKey);

        var httpResponse = await httpClient.SendAsync(requestMessage, cancellationToken);

        return await httpResponse.Content.ReadAsStringAsync();
    }

    public async static Task<string> GetTopicProfileContentAsync(
        this HttpClient httpClient,
        string accessKey,
        int topicId,
        CancellationToken cancellationToken = default)
    {
        var requestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            $"/api/open/topic/detail?topicId={topicId}");

        requestMessage.Headers.TryAddWithoutValidation("access-key", accessKey);

        var httpResponse = await httpClient.SendAsync(requestMessage, cancellationToken);

        return await httpResponse.Content.ReadAsStringAsync();
    }

    public async static Task<string> GetTopicForMeProfileContentAsync(
        this HttpClient httpClient,
        string accessKey,
        int topicId,
        CancellationToken cancellationToken = default)
    {
        var requestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            $"/api/open/topic/joinTopicDetail?topicId={topicId}");

        requestMessage.Headers.TryAddWithoutValidation("access-key", accessKey);

        var httpResponse = await httpClient.SendAsync(requestMessage, cancellationToken);

        return await httpResponse.Content.ReadAsStringAsync();
    }

    private const string _createTopicTemplate = "{\"topicCode\":\"$topicCode\",\"topicName\":\"$topicName\",\"contact\":\"$contact\",\"introduction\":\"$introduction\",\"receiptMessage\":\"$receiptMessage\"}";
    public async static Task<string> GetCreateTopicContentAsync(
        this HttpClient httpClient,
        string accessKey,
        string topicCode,
        string topicName,
        string contact,
        string introduction,
        string receiptMessage = "",
        CancellationToken cancellationToken = default)
    {
        var requestMessage = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/open/topic/add");

        var requestBody = _createTopicTemplate
            .Replace("$topicCode", topicCode)
            .Replace("$topicName", topicName)
            .Replace("$contact", contact)
            .Replace("$introduction", introduction)
            .Replace("$receiptMessage", receiptMessage);

        requestMessage.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
        requestMessage.Headers.TryAddWithoutValidation("access-key", accessKey);

        var httpResponse = await httpClient.SendAsync(requestMessage, cancellationToken);

        return await httpResponse.Content.ReadAsStringAsync();
    }

    public async static Task<string> GetTopicQrCodeContentAsync(
        this HttpClient httpClient,
        string accessKey,
        int topicId,
        PushPlusTopicQrCodeType forever = PushPlusTopicQrCodeType.Temporary,
        CancellationToken cancellationToken = default)
    {
        var requestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            $"/api/open/topic/qrCode?topicId={topicId}&forever={(int)forever}");

        requestMessage.Headers.TryAddWithoutValidation("access-key", accessKey);

        var httpResponse = await httpClient.SendAsync(requestMessage, cancellationToken);

        return await httpResponse.Content.ReadAsStringAsync();
    }

    public async static Task<string> GetQuitTopicContentAsync(
        this HttpClient httpClient,
        string accessKey,
        int topicId,
        CancellationToken cancellationToken = default)
    {
        var requestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            $"/api/open/topic/exitTopic?topicId={topicId}");

        requestMessage.Headers.TryAddWithoutValidation("access-key", accessKey);

        var httpResponse = await httpClient.SendAsync(requestMessage, cancellationToken);

        return await httpResponse.Content.ReadAsStringAsync();
    }

    private const string _getSubscriberListTemplate = "{\"current\":$current,\"pageSize\":$pageSize,\"params\":{\"topicId\":$topicId}}";
    public async static Task<string> GetSubscriberListContentAsync(
        this HttpClient httpClient,
        string accessKey,
        int current,
        int pageSize,
        int topicId,
        CancellationToken cancellationToken = default)
    {
        var requestMessage = new HttpRequestMessage(
             HttpMethod.Post,
             "/api/open/topicUser/subscriberList");

        var requestBody = _getSubscriberListTemplate
            .Replace("$current", current.ToString())
            .Replace("$pageSize", pageSize.ToString())
            .Replace("$topicId", topicId.ToString());

        requestMessage.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
        requestMessage.Headers.TryAddWithoutValidation("access-key", accessKey);

        var httpResponse = await httpClient.SendAsync(requestMessage, cancellationToken);

        return await httpResponse.Content.ReadAsStringAsync();
    }

    public async static Task<string> GetUnSubscriberContentAsync(
        this HttpClient httpClient,
        string accessKey,
        int topicRelationId,
        CancellationToken cancellationToken = default)
    {
        var requestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            $"/api/open/topicUser/deleteTopicUser?topicRelationId={topicRelationId}");

        requestMessage.Headers.TryAddWithoutValidation("access-key", accessKey);

        var httpResponse = await httpClient.SendAsync(requestMessage, cancellationToken);

        return await httpResponse.Content.ReadAsStringAsync();
    }
}
