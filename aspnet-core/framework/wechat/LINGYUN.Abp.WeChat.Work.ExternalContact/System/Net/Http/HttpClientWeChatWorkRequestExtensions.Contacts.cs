using LINGYUN.Abp.WeChat.Work.ExternalContact.Contacts.Request;
using LINGYUN.Abp.WeChat.Work.ExternalContact.Contacts.Response;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;
internal static partial class HttpClientWeChatWorkRequestExtensions
{
    public async static Task<WeChatWorkGetExternalContactListResponse> GetExternalContactListAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkGetExternalContactListRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/contact_list");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson())
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkGetExternalContactListResponse>();
    }
}
