using LINGYUN.Abp.WeChat.Work.Token.Models;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http
{
    internal static partial class HttpClientWeChatWorkRequestExtensions
    {
        public async static Task<HttpResponseMessage> GetTokenAsync(this HttpMessageInvoker client, WeChatWorkTokenRequest request, CancellationToken cancellationToken = default)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append("/cgi-bin/gettoken");
            urlBuilder.AppendFormat("?corpid={0}", request.CorpId);
            urlBuilder.AppendFormat("&corpsecret={0}", request.CorpSecret);

            var httpRequest = new HttpRequestMessage(HttpMethod.Get, urlBuilder.ToString());

            return await client.SendAsync(httpRequest, cancellationToken);
        }
    }
}
