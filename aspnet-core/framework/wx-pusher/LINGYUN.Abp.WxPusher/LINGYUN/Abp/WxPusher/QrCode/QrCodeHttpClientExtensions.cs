using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WxPusher.QrCode;

internal static class QrCodeHttpClientExtensions
{
    public async static Task<string> CreateQrcodeAsync(
        this HttpClient httpClient,
        CreateQrcodeRequest qrcodeRequest,
        CancellationToken cancellationToken = default)
    {
        var requestMessage = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/fun/create/qrcode");

        var requestBody = JsonConvert.SerializeObject(qrcodeRequest);
        requestMessage.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

        var httpResponse = await httpClient.SendAsync(requestMessage, cancellationToken);

        return await httpResponse.Content.ReadAsStringAsync();
    }

    public async static Task<string> GetScanQrCodeUidAsync(
        this HttpClient httpClient,
        string code,
        CancellationToken cancellationToken = default)
    {
        var requestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            $"/api/fun/scan-qrcode-uid?code={code}");

        var httpResponse = await httpClient.SendAsync(requestMessage, cancellationToken);

        return await httpResponse.Content.ReadAsStringAsync();
    }
}
