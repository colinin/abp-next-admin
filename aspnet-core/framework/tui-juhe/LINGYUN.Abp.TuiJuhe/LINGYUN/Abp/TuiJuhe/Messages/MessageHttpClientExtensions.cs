using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.TuiJuhe.Messages;

internal static class MessageHttpClientExtensions
{
    public async static Task<string> SendMessageAsync(
        this HttpClient httpClient,
        string token,
        string title,
        string content,
        string serviceId,
        string docType = "txt",
        CancellationToken cancellationToken = default)
    {
        var requestMessage = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/plus/pushApi");

        var requestData = new Dictionary<string, string>
        {
            { "token", token },
            { "title", title },
            { "content", content },
            { "service_id", serviceId },
            { "doc_type", docType ?? "txt" },
        };
        var formData = new FormUrlEncodedContent(requestData);
        requestMessage.Content = formData;

        var httpResponse = await httpClient.SendAsync(requestMessage, cancellationToken);

        return await httpResponse.Content.ReadAsStringAsync();
    }
}
