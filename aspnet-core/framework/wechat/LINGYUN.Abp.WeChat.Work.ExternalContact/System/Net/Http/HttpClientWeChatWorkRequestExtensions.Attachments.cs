using LINGYUN.Abp.WeChat.Work.ExternalContact.Attachments.Request;
using LINGYUN.Abp.WeChat.Work.ExternalContact.Attachments.Response;
using LINGYUN.Abp.WeChat.Work.Utils;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;
internal static partial class HttpClientWeChatWorkRequestExtensions
{
    public async static Task<WeChatWorkUploadAttachmentResponse> UploadAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkUploadAttachmentRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/media/upload_attachment");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);
        urlBuilder.AppendFormat("&media_type={0}", request.MediaType.ToString().ToLowerInvariant());
        urlBuilder.AppendFormat("&attachment_type={0}", request.AttachmentType);

        var fileBytes = await request.Content.GetStream().GetAllBytesAsync();
        using var httpRequest = new HttpRequestMessage(
            HttpMethod.Post,
            urlBuilder.ToString())
        {
            Content = WeChatWorkHttpContentBuildHelper.BuildUploadMediaContent("media", fileBytes, request.Content.FileName)
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkUploadAttachmentResponse>();
    }
}
