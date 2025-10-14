using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;
internal static partial class HttpClientWeChatWorkRequestExtensions
{
    public async static Task<HttpResponseMessage> GetServerDomainIpAsync(
        this HttpMessageInvoker client,
        string accessToken,
        CancellationToken cancellationToken = default)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Get,
            $"/cgi-bin/security/get_server_domain_ip?access_token={accessToken}"); ;

        return await client.SendAsync(httpRequest, cancellationToken);
    }
}
