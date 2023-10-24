using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WxPusher.Messages;

internal static class MessageHttpClientExtensions
{
    public async static Task<string> SendMessageAsync(
        this HttpClient httpClient,
        SendMessage sendMessage,
        CancellationToken cancellationToken = default)
    {
        var requestMessage = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/send/message");

        var requestBody = JsonConvert.SerializeObject(sendMessage);
        requestMessage.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

        var httpResponse = await httpClient.SendAsync(requestMessage, cancellationToken);

        return await httpResponse.Content.ReadAsStringAsync();
    }

    public async static Task<string> QueryMessageAsync(
        this HttpClient httpClient,
        int messageId,
        CancellationToken cancellationToken = default)
    {
        var requestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            $"/api/send/query/{messageId}");

        var httpResponse = await httpClient.SendAsync(requestMessage, cancellationToken);

        return await httpResponse.Content.ReadAsStringAsync();
    }
}
