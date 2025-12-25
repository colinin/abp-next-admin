using LINGYUN.Abp.WeChat.Work.ExternalContact.MsgAudits.Request;
using LINGYUN.Abp.WeChat.Work.ExternalContact.MsgAudits.Response;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;
internal static partial class HttpClientWeChatWorkRequestExtensions
{
    public async static Task<WeChatWorkGetPermitUserListResponse> GetPermitUserListAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkGetPermitUserListRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/msgaudit/get_permit_user_list");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson())
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkGetPermitUserListResponse>();
    }

    public async static Task<WeChatWorkCheckSingleAgreeResponse> CheckSingleAgreeAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkCheckSingleAgreeRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/msgaudit/check_single_agree");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson())
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkCheckSingleAgreeResponse>();
    }

    public async static Task<WeChatWorkCheckRoomAgreeResponse> CheckRoomAgreeAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkCheckRoomAgreeRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/msgaudit/check_room_agree");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson())
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkCheckRoomAgreeResponse>();
    }

    public async static Task<WeChatWorkGetGroupChatResponse> GetGroupChatAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkGetGroupChatRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/msgaudit/groupchat/get");
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
}
