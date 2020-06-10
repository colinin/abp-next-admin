using LINGYUN.Abp.WeChat.Authorization;
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
            getResuestUrlBuilder.Append("?grant_type=client_credential");
            getResuestUrlBuilder.AppendFormat("&appid={0}", request.AppId);
            getResuestUrlBuilder.AppendFormat("&secret={0}", request.AppSecret);

            var getRequest = new HttpRequestMessage(HttpMethod.Get, getResuestUrlBuilder.ToString());
            HttpResponseMessage httpResponse;

            httpResponse = await client.SendAsync(getRequest, cancellationToken).ConfigureAwait(false);

            return httpResponse;
        }
    }
}
