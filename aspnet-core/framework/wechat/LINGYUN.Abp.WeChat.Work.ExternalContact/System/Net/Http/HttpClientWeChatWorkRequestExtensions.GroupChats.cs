using LINGYUN.Abp.WeChat.Work.ExternalContact.Customers.Request;
using LINGYUN.Abp.WeChat.Work.ExternalContact.GroupChats.Request;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;
internal static partial class HttpClientWeChatWorkRequestExtensions
{
    public async static Task<HttpResponseMessage> GetGroupChatListAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkGetGroupChatListRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/groupchat/list");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson())
        };

        return await client.SendAsync(httpRequest, cancellationToken);
    }

    public async static Task<HttpResponseMessage> GetGroupChatAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkGetGroupChatRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/groupchat/get");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson())
        };

        return await client.SendAsync(httpRequest, cancellationToken);
    }

    public async static Task<HttpResponseMessage> OpengIdToChatIdAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkOpengIdToChatIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/opengid_to_chatid");
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
