using LINGYUN.Abp.WeChat.Work.Chat.Request;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http
{
    internal static partial class HttpClientWeChatWorkRequestExtensions
    {
        public async static Task<HttpResponseMessage> GetAppChatAsync(
            this HttpMessageInvoker client,
            string accessToken,
            string chatId,
            CancellationToken cancellationToken = default)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append("/cgi-bin/appchat/get");
            urlBuilder.AppendFormat("?access_token={0}", accessToken);
            urlBuilder.AppendFormat("&chatid={0}", chatId);

            var httpRequest = new HttpRequestMessage(
                HttpMethod.Get,
                urlBuilder.ToString());

            return await client.SendAsync(httpRequest, cancellationToken);
        }

        public async static Task<HttpResponseMessage> CreateAppChatAsync(
            this HttpMessageInvoker client,
            string accessToken,
            WeChatWorkAppChatCreateRequest request,
            CancellationToken cancellationToken = default)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append("/cgi-bin/appchat/create");
            urlBuilder.AppendFormat("?access_token={0}", accessToken);

            var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                urlBuilder.ToString())
            {
                Content = new StringContent(request.SerializeToJson())
            };

            return await client.SendAsync(httpRequest, cancellationToken);
        }

        public async static Task<HttpResponseMessage> UpdateAppChatAsync(
            this HttpMessageInvoker client,
            string accessToken,
            WeChatWorkAppChatUpdateRequest request,
            CancellationToken cancellationToken = default)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append("/cgi-bin/appchat/update");
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
