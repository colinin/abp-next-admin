using LINGYUN.Abp.WeChat.Work;
using LINGYUN.Abp.WeChat.Work.Chat.Request;
using LINGYUN.Abp.WeChat.Work.Chat.Response;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

internal static partial class HttpClientWeChatWorkRequestExtensions
{
    public async static Task<WeChatWorkAppChatInfoResponse> GetAppChatAsync(
        this HttpMessageInvoker client,
        string accessToken,
        string chatId,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/appchat/get");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);
        urlBuilder.AppendFormat("&chatid={0}", chatId);

        using var httpRequest = new HttpRequestMessage(
            HttpMethod.Get,
            urlBuilder.ToString());

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkAppChatInfoResponse>();
    }

    public async static Task<WeChatWorkAppChatCreateResponse> CreateAppChatAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkAppChatCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/appchat/create");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
            HttpMethod.Post,
            urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson())
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkAppChatCreateResponse>();
    }

    public async static Task<WeChatWorkResponse> UpdateAppChatAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkAppChatUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/appchat/update");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
            HttpMethod.Post,
            urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson())
        };
        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkResponse>();
    }
}
