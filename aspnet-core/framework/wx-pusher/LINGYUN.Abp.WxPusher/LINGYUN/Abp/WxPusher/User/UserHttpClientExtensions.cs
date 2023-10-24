using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WxPusher.User;

internal static class UserHttpClientExtensions
{
    public async static Task<string> GetUserListAsync(
        this HttpClient httpClient,
        string appToken,
        int page = 1,
        int pageSize = 10,
        string uid = null,
        bool? isBlock = null,
        FlowType? type = null,
        CancellationToken cancellationToken = default)
    {
        var requestUrl = "/api/fun/wxuser/v2?appToken=$appToken&page=$page&pageSize=$pageSize&uid=$uid&isBlock=$isBlock&type=$type";

        requestUrl = requestUrl
            .Replace("$appToken", appToken)
            .Replace("$page", page.ToString())
            .Replace("$pageSize", pageSize.ToString())
            .Replace("$uid", uid ?? "")
            .Replace("$isBlock", isBlock?.ToString() ?? "")
            .Replace("$type", type.HasValue ? ((int)type).ToString() : "");

        var requestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            requestUrl);

        var httpResponse = await httpClient.SendAsync(requestMessage, cancellationToken);

        return await httpResponse.Content.ReadAsStringAsync();
    }

    public async static Task<string> DeleteUserAsync(
        this HttpClient httpClient,
        string appToken,
        int id,
        CancellationToken cancellationToken = default)
    {
        var requestMessage = new HttpRequestMessage(
            HttpMethod.Delete,
            $"/api/fun/remove?appToken={appToken}&id={id}");

        var httpResponse = await httpClient.SendAsync(requestMessage, cancellationToken);

        return await httpResponse.Content.ReadAsStringAsync();
    }

    public async static Task<string> RejectUserAsync(
        this HttpClient httpClient,
        string appToken,
        int id,
        bool reject,
        CancellationToken cancellationToken = default)
    {
        var requestMessage = new HttpRequestMessage(
            HttpMethod.Put,
            $"/api/fun/reject?appToken={appToken}&id={id}&reject={reject}");

        var httpResponse = await httpClient.SendAsync(requestMessage, cancellationToken);

        return await httpResponse.Content.ReadAsStringAsync();
    }
}
