using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.PushPlus.Token;

internal static class TokenHttpClientExtensions
{
    public async static Task<string> GetTokenContentAsync(
        this HttpClient httpClient,
        string token,
        string secretKey,
        CancellationToken cancellationToken = default)
    {
        var requestMessage = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/common/openApi/getAccessKey");
        var requestBodyTemplate = "{\"token\":\"$token\",\"secretKey\":\"$secretKey\"}";
        var requestBody = requestBodyTemplate
            .Replace("$token", token)
            .Replace("$secretKey", secretKey);

        requestMessage.Content = new StringContent(requestBody);

        var httpResponse = await httpClient.SendAsync(requestMessage, cancellationToken);

        return await httpResponse.Content.ReadAsStringAsync();
    }
}
