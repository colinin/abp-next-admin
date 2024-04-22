using LINGYUN.Abp.WeChat.Work.Messages;
using LINGYUN.Abp.WeChat.Work.Messages.Request;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http
{
    internal static partial class HttpClientWeChatWorkRequestExtensions
    {
        public async static Task<HttpResponseMessage> SendMessageAsync(
            this HttpMessageInvoker client,
            WeChatWorkMessageRequest<WeChatWorkMessage> request,
            CancellationToken cancellationToken = default)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append("/cgi-bin/message/send");
            urlBuilder.AppendFormat("?access_token={0}", request.AccessToken);

            var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                urlBuilder.ToString())
            {
                Content = new StringContent(request.Message.SerializeToJson())
            };

            return await client.SendAsync(httpRequest, cancellationToken);
        }

        public async static Task<HttpResponseMessage> SendMessageAsync(
            this HttpMessageInvoker client,
            WeChatWorkMessageRequest<WeChatWorkAppChatMessage> request,
            CancellationToken cancellationToken = default)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append("/cgi-bin/appchat/send");
            urlBuilder.AppendFormat("?access_token={0}", request.AccessToken);

            var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                urlBuilder.ToString())
            {
                Content = new StringContent(request.Message.SerializeToJson())
            };

            return await client.SendAsync(httpRequest, cancellationToken);
        }

        public async static Task<HttpResponseMessage> ReCallMessageAsync(
            this HttpMessageInvoker client,
            WeChatWorkMessageReCallRequest request,
            CancellationToken cancellationToken = default)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append("/cgi-bin/message/recall");
            urlBuilder.AppendFormat("?access_token={0}", request.AccessToken);

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
