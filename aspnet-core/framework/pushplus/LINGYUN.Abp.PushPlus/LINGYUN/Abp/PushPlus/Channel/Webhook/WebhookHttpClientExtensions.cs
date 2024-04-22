using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.PushPlus.Channel.Webhook;

internal static class WebhookHttpClientExtensions
{
    private const string _getWebhookListTemplate = "{\"current\":$current,\"pageSize\":$pageSize}";
    public async static Task<string> GetWebhookListContentAsync(
        this HttpClient httpClient,
        string accessKey,
        int current,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var requestMessage = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/open/webhook/list");

        var requestBody = _getWebhookListTemplate
            .Replace("$current", current.ToString())
            .Replace("$pageSize", pageSize.ToString());

        requestMessage.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
        requestMessage.Headers.TryAddWithoutValidation("access-key", accessKey);

        var httpResponse = await httpClient.SendAsync(requestMessage, cancellationToken);

        return await httpResponse.Content.ReadAsStringAsync();
    }

    public async static Task<string> GetWebhookContentAsync(
        this HttpClient httpClient,
        string accessKey,
        int webhookId,
        CancellationToken cancellationToken = default)
    {
        var requestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            $"/api/open/webhook/detail?webhookId={webhookId}");

        requestMessage.Headers.TryAddWithoutValidation("access-key", accessKey);

        var httpResponse = await httpClient.SendAsync(requestMessage, cancellationToken);

        return await httpResponse.Content.ReadAsStringAsync();
    }

    private const string _createWebhookTemplate = "{\"webhookCode\":\"$webhookCode\",\"webhookName\":\"$webhookName\",\"webhookType\":$webhookType,\"webhookUrl\":\"$webhookUrl\"}";
    public async static Task<string> GetCreateWebhookContentAsync(
        this HttpClient httpClient,
        string accessKey,
        string webhookCode,
        string webhookName,
        PushPlusWebhookType webhookType,
        string webhookUrl,
        CancellationToken cancellationToken = default)
    {
        var requestMessage = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/open/webhook/add");

        var requestBody = _createWebhookTemplate
            .Replace("$webhookCode", webhookCode)
            .Replace("$webhookName", webhookName)
            .Replace("$webhookType", ((int)webhookType).ToString())
            .Replace("$webhookUrl", webhookUrl);

        requestMessage.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
        requestMessage.Headers.TryAddWithoutValidation("access-key", accessKey);

        var httpResponse = await httpClient.SendAsync(requestMessage, cancellationToken);

        return await httpResponse.Content.ReadAsStringAsync();
    }

    private const string _updateWebhookTemplate = "{\"id\":$id,\"webhookCode\":\"$webhookCode\",\"webhookName\":\"$webhookName\",\"webhookType\":$webhookType,\"webhookUrl\":\"$webhookUrl\"}";
    public async static Task<string> GetUpdateWebhookContentAsync(
        this HttpClient httpClient,
        string accessKey,
        int id,
        string webhookCode,
        string webhookName,
        PushPlusWebhookType webhookType,
        string webhookUrl,
        CancellationToken cancellationToken = default)
    {
        var requestMessage = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/open/topic/edit");

        var requestBody = _updateWebhookTemplate
            .Replace("$id", id.ToString())
            .Replace("$webhookCode", webhookCode)
            .Replace("$webhookName", webhookName)
            .Replace("$webhookType", ((int)webhookType).ToString())
            .Replace("$webhookUrl", webhookUrl);

        requestMessage.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
        requestMessage.Headers.TryAddWithoutValidation("access-key", accessKey);

        var httpResponse = await httpClient.SendAsync(requestMessage, cancellationToken);

        return await httpResponse.Content.ReadAsStringAsync();
    }
}
