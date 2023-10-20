using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.PushPlus.Message;

internal static class MessageHttpClientExtensions
{
    private const string _sendMessagTemplate = "{\"token\":\"$token\",\"title\":\"$title\",\"content\":\"$content\",\"topic\":\"$topic\",\"template\":\"$template\",\"channel\":\"$channel\",\"webhook\":\"$webhook\",\"callbackUrl\":\"$callbackUrl\",\"timestamp\":\"$timestamp\"}";

    public async static Task<string> GetSendMessageContentAsync(
        this HttpClient httpClient,
        string token,
        string title,
        string content,
        string topic = "",
        string template = "html",
        string channel = "wechat",
        string webhook = "",
        string callbackUrl = "",
        string timestamp = "",
        CancellationToken cancellationToken = default)
    {
        var requestMessage = new HttpRequestMessage(
            HttpMethod.Post,
            "/send");

        var requestBody = _sendMessagTemplate
            .Replace("$token", token)
            .Replace("$title", title)
            .Replace("$content", content)
            .Replace("$topic", topic)
            .Replace("$template", template)
            .Replace("$channel", channel)
            .Replace("$webhook", webhook)
            .Replace("$callbackUrl", callbackUrl)
            .Replace("$timestamp", timestamp);

        requestMessage.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

        var httpResponse = await httpClient.SendAsync(requestMessage, cancellationToken);

        return await httpResponse.Content.ReadAsStringAsync();
    }


    public async static Task<string> GetSendResultContentAsync(
        this HttpClient httpClient,
        string accessKey,
        string shortCode,
        CancellationToken cancellationToken = default)
    {
        var requestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            $"/api/open/message/sendMessageResult?shortCode={shortCode}");

        requestMessage.Headers.TryAddWithoutValidation("access-key", accessKey);

        var httpResponse = await httpClient.SendAsync(requestMessage, cancellationToken);

        return await httpResponse.Content.ReadAsStringAsync();
    }

    public async static Task<string> GetMessageListContentAsync(
        this HttpClient httpClient,
        string accessKey,
        int current,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var requestMessage = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/open/message/list");

        var requestBodyTemplate = "{\"current\":\"$current\",\"pageSize\":\"$pageSize\"}";
        var requestBody = requestBodyTemplate
            .Replace("$current", current.ToString())
            .Replace("$pageSize", pageSize.ToString());

        requestMessage.Content = new StringContent(requestBody);
        requestMessage.Headers.TryAddWithoutValidation("access-key", accessKey);

        var httpResponse = await httpClient.SendAsync(requestMessage, cancellationToken);

        return await httpResponse.Content.ReadAsStringAsync();
    }
}
