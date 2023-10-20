using LINGYUN.Abp.WeChat.Work.Media.Models;
using LINGYUN.Abp.WeChat.Work.Utils;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http
{
    internal static partial class HttpClientWeChatWorkRequestExtensions
    {
        public async static Task<HttpResponseMessage> GetMediaAsync(
            this HttpMessageInvoker client,
            string accessToken,
            string mediaId,
            CancellationToken cancellationToken = default)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append("/cgi-bin/media/get");
            urlBuilder.AppendFormat("?access_token={0}", accessToken);
            urlBuilder.AppendFormat("&media_id={0}", mediaId);

            var httpRequest = new HttpRequestMessage(HttpMethod.Get, urlBuilder.ToString());

            return await client.SendAsync(httpRequest, cancellationToken);
        }

        public async static Task<HttpResponseMessage> UploadMediaAsync(
            this HttpMessageInvoker client,
            string type,
            WeChatWorkMediaRequest request,
            CancellationToken cancellationToken = default)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append("/cgi-bin/media/upload");
            urlBuilder.AppendFormat("?access_token={0}", request.AccessToken);
            urlBuilder.AppendFormat("&type={0}", type);

            var fileBytes = await request.Content.GetStream().GetAllBytesAsync();
            var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                urlBuilder.ToString())
            {
                Content = HttpContentBuildHelper.BuildUploadMediaContent("media", fileBytes, request.Content.FileName)
            };

            return await client.SendAsync(httpRequest, cancellationToken);
        }

        public async static Task<HttpResponseMessage> UploadImageAsync(
            this HttpMessageInvoker client,
            WeChatWorkMediaRequest request,
            CancellationToken cancellationToken = default)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append("/cgi-bin/media/uploadimg");
            urlBuilder.AppendFormat("?access_token={0}", request.AccessToken);

            var fileBytes = await request.Content.GetStream().GetAllBytesAsync();
            var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                urlBuilder.ToString())
            {
                Content = HttpContentBuildHelper.BuildUploadMediaContent("file", fileBytes, request.Content.FileName)
            };

            return await client.SendAsync(httpRequest, cancellationToken);
        }
    }
}
