using LINGYUN.Abp.WeChat.Work.Security.Models;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;
internal static partial class HttpClientWeChatWorkRequestExtensions
{
    public async static Task<WeChatServerDomainResponse> GetServerDomainIpAsync(
        this HttpMessageInvoker client,
        string accessToken,
        CancellationToken cancellationToken = default)
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Get,
            $"/cgi-bin/security/get_server_domain_ip?access_token={accessToken}"); ;

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatServerDomainResponse>();
    }
}
