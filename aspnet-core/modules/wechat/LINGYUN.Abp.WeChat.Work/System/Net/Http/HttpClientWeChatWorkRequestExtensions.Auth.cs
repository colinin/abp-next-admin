using LINGYUN.Abp.WeChat.Work.Authorize.Request;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http
{
    internal static partial class HttpClientWeChatWorkRequestExtensions
    {
        public async static Task<HttpResponseMessage> GetUserInfoAsync(
            this HttpMessageInvoker client,
            string accessToken,
            string code,
            CancellationToken cancellationToken = default)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append("/cgi-bin/auth/getuserinfo");
            urlBuilder.AppendFormat("?access_token={0}", accessToken);
            urlBuilder.AppendFormat("&code={0}", code);

            var httpRequest = new HttpRequestMessage(HttpMethod.Get, urlBuilder.ToString());

            return await client.SendAsync(httpRequest, cancellationToken);
        }

        public async static Task<HttpResponseMessage> GetUserDetailAsync(
            this HttpMessageInvoker client,
            string accessToken,
            WeChatWorkUserDetailRequest request,
            CancellationToken cancellationToken = default)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append("/cgi-bin/auth/getuserdetail");
            urlBuilder.AppendFormat("?access_token={0}", accessToken);

            var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                urlBuilder.ToString())
            {
                Content = new StringContent(request.SerializeToJson())
            };

            return await client.SendAsync(httpRequest, cancellationToken);
        }
    }
}
