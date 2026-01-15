using LINGYUN.Abp.WeChat.Work.ExternalContact.GroupChats.Request;
using LINGYUN.Abp.WeChat.Work.ExternalContact.GroupChats.Response;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;
internal static partial class HttpClientWeChatWorkRequestExtensions
{
    public async static Task<WeChatWorkGetGroupChatListResponse> GetGroupChatListAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkGetGroupChatListRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/groupchat/list");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson())
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkGetGroupChatListResponse>();
    }

    public async static Task<WeChatWorkGetGroupChatResponse> GetGroupChatAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkGetGroupChatRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/groupchat/get");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson())
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkGetGroupChatResponse>();
    }

    public async static Task<WeChatWorkOpengIdToChatIdResponse> OpengIdToChatIdAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkOpengIdToChatIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/opengid_to_chatid");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson())
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkOpengIdToChatIdResponse>();
    }
}
