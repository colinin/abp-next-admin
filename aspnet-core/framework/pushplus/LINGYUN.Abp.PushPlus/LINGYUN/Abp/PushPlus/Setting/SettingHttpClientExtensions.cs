using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.PushPlus.Setting;

internal static class SettingHttpClientExtensions
{
    public async static Task<string> GetDefaultChannelContentAsync(
        this HttpClient httpClient,
        string accessKey,
        CancellationToken cancellationToken = default)
    {
        var requestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            "/api/open/setting/getUserSettings");
        requestMessage.Headers.TryAddWithoutValidation("access-key", accessKey);

        var httpResponse = await httpClient.SendAsync(requestMessage, cancellationToken);

        return await httpResponse.Content.ReadAsStringAsync();
    }

    private const string _updateDefaultChannelTemplate = "{\"defaultChannel\":\"$defaultChannel\",\"defaultWebhook\":\"$defaultWebhook\"}";
    public async static Task<string> GetUpdateDefaultChannelContentAsync(
        this HttpClient httpClient,
        string accessKey,
        string defaultChannel,
        string defaultWebhook = "",
        CancellationToken cancellationToken = default)
    {
        var requestMessage = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/open/setting/changeDefaultChannel");

        var requestBody = _updateDefaultChannelTemplate
            .Replace("$defaultChannel", defaultChannel)
            .Replace("$defaultWebhook", defaultWebhook);
        requestMessage.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

        requestMessage.Headers.TryAddWithoutValidation("access-key", accessKey);

        var httpResponse = await httpClient.SendAsync(requestMessage, cancellationToken);

        return await httpResponse.Content.ReadAsStringAsync();
    }

    public async static Task<string> GetUpdateRecevieLimitContentAsync(
        this HttpClient httpClient,
        string accessKey,
        PushPlusChannelRecevieLimit recevieLimit,
        CancellationToken cancellationToken = default)
    {
        var requestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            $"/api/open/setting/changeRecevieLimit?recevieLimit=${(int)recevieLimit}");

        requestMessage.Headers.TryAddWithoutValidation("access-key", accessKey);

        var httpResponse = await httpClient.SendAsync(requestMessage, cancellationToken);

        return await httpResponse.Content.ReadAsStringAsync();
    }
}
