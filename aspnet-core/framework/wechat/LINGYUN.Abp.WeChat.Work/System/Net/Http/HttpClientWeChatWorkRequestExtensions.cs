using LINGYUN.Abp.WeChat.Work.Token.Models;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

internal static partial class HttpClientWeChatWorkRequestExtensions
{
    public async static Task<WeChatWorkTokenResponse> GetTokenAsync(this HttpMessageInvoker client, WeChatWorkTokenRequest request, CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/gettoken");
        urlBuilder.AppendFormat("?corpid={0}", request.CorpId);
        urlBuilder.AppendFormat("&corpsecret={0}", request.CorpSecret);

        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, urlBuilder.ToString());

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkTokenResponse>();
    }
}
