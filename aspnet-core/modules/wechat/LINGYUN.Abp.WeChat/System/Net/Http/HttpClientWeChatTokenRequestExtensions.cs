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
            var getResuestUrlBuilder = new StringBuilder();
            getResuestUrlBuilder.Append(request.BaseUrl);
            getResuestUrlBuilder.Append("cgi-bin/token");
            getResuestUrlBuilder.Append("?grant_type=client_credential");
            getResuestUrlBuilder.AppendFormat("&appid={0}", request.AppId);
            getResuestUrlBuilder.AppendFormat("&secret={0}", request.AppSecret);

            var getRequest = new HttpRequestMessage(HttpMethod.Get, getResuestUrlBuilder.ToString());
            HttpResponseMessage httpResponse;

            httpResponse = await client.SendAsync(getRequest, cancellationToken).ConfigureAwait(false);

            return httpResponse;
        }

        public static async Task<HttpResponseMessage> RequestWeChatOpenIdAsync(this HttpMessageInvoker client, WeChatOpenIdRequest request, CancellationToken cancellationToken = default)
        {
            var getResuestUrlBuiilder = new StringBuilder();
            getResuestUrlBuiilder.Append(request.BaseUrl);
            getResuestUrlBuiilder.Append("sns/jscode2session");
            getResuestUrlBuiilder.AppendFormat("?appid={0}", request.AppId);
            getResuestUrlBuiilder.AppendFormat("&secret={0}", request.Secret);
            getResuestUrlBuiilder.AppendFormat("&js_code={0}", request.Code);
            getResuestUrlBuiilder.Append("&grant_type=authorization_code");

            var getRequest = new HttpRequestMessage(HttpMethod.Get, getResuestUrlBuiilder.ToString());
            HttpResponseMessage httpResponse;

            httpResponse = await client.SendAsync(getRequest, cancellationToken).ConfigureAwait(false);

            return httpResponse;
        }
    }
}
