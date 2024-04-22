using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.PushPlus.User;

internal static class UserHttpClientExtensions
{
    public async static Task<string> GetTokenContentAsync(
        this HttpClient httpClient,
        string accessKey,
        CancellationToken cancellationToken = default)
    {
        var requestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            "/api/open/user/token");

        requestMessage.Headers.TryAddWithoutValidation("access-key", accessKey);

        var httpResponse = await httpClient.SendAsync(requestMessage, cancellationToken);

        return await httpResponse.Content.ReadAsStringAsync();
    }

    public async static Task<string> GetProfileContentAsync(
        this HttpClient httpClient,
        string accessKey,
        CancellationToken cancellationToken = default)
    {
        var requestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            "api/open/user/myInfo");

        requestMessage.Headers.TryAddWithoutValidation("access-key", accessKey);

        var httpResponse = await httpClient.SendAsync(requestMessage, cancellationToken);

        return await httpResponse.Content.ReadAsStringAsync();
    }

    public async static Task<string> GetLimitTimeContentAsync(
        this HttpClient httpClient,
        string accessKey,
        CancellationToken cancellationToken = default)
    {
        var requestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            "api/open/user/userLimitTime");

        requestMessage.Headers.TryAddWithoutValidation("access-key", accessKey);

        var httpResponse = await httpClient.SendAsync(requestMessage, cancellationToken);

        return await httpResponse.Content.ReadAsStringAsync();
    }
}
