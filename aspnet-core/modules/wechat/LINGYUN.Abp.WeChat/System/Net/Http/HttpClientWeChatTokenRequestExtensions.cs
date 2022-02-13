using LINGYUN.Abp.WeChat.OpenId;
using LINGYUN.Abp.WeChat.Token;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http
{
    public static class HttpClientWeChatTokenRequestExtensions
    {
        public static async Task<HttpResponseMessage> RequestWeChatCodeTokenAsync(this HttpMessageInvoker client, WeChatTokenRequest request, CancellationToken cancellationToken = default)
        {
            var getRequestUrlBuilder = new StringBuilder();
            getRequestUrlBuilder.Append(request.BaseUrl);
            getRequestUrlBuilder.Append("cgi-bin/token");
            getRequestUrlBuilder.Append("?grant_type=client_credential");
            getRequestUrlBuilder.AppendFormat("&appid={0}", request.AppId);
            getRequestUrlBuilder.AppendFormat("&secret={0}", request.AppSecret);

            var getRequest = new HttpRequestMessage(HttpMethod.Get, getRequestUrlBuilder.ToString());
            HttpResponseMessage httpResponse;

            httpResponse = await client.SendAsync(getRequest, cancellationToken).ConfigureAwait(false);

            return httpResponse;
        }

        public static async Task<HttpResponseMessage> RequestWeChatOpenIdAsync(this HttpMessageInvoker client, WeChatOpenIdRequest request, CancellationToken cancellationToken = default)
        {
            var getRequestUrlBuilder = new StringBuilder();
            getRequestUrlBuilder.Append(request.BaseUrl);
            getRequestUrlBuilder.Append("sns/jscode2session");
            getRequestUrlBuilder.AppendFormat("?appid={0}", request.AppId);
            getRequestUrlBuilder.AppendFormat("&secret={0}", request.Secret);
            getRequestUrlBuilder.AppendFormat("&js_code={0}", request.Code);
            getRequestUrlBuilder.Append("&grant_type=authorization_code");

            var getRequest = new HttpRequestMessage(HttpMethod.Get, getRequestUrlBuilder.ToString());
            HttpResponseMessage httpResponse;

            httpResponse = await client.SendAsync(getRequest, cancellationToken).ConfigureAwait(false);

            return httpResponse;
        }
    }
}
