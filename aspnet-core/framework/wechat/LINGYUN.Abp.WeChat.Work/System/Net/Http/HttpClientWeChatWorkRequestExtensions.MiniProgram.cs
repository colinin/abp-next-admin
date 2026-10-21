using LINGYUN.Abp.WeChat.Work.Authorize.Response;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

internal static partial class HttpClientWeChatWorkRequestExtensions
{
    public async static Task<WeChatWorkCode2SessionResponse> Code2SessionAsync(
        this HttpMessageInvoker client,
        string accessToken,
        string jsCode,
        string grantType = "authorization_code",
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/miniprogram/jscode2session");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);
        urlBuilder.AppendFormat("&js_code={0}", jsCode);
        urlBuilder.AppendFormat("&grant_type={0}", grantType);

        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, urlBuilder.ToString());

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkCode2SessionResponse>();
    }
}
