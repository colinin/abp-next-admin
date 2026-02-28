using LINGYUN.Abp.WeChat.Work;
using LINGYUN.Abp.WeChat.Work.Messages;
using LINGYUN.Abp.WeChat.Work.Messages.Request;
using LINGYUN.Abp.WeChat.Work.Messages.Response;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

internal static partial class HttpClientWeChatWorkRequestExtensions
{
    public async static Task<WeChatWorkMessageResponse> SendMessageAsync(
        this HttpMessageInvoker client,
        WeChatWorkMessageRequest<WeChatWorkMessage> request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/message/send");
        urlBuilder.AppendFormat("?access_token={0}", request.AccessToken);

        using var httpRequest = new HttpRequestMessage(
            HttpMethod.Post,
            urlBuilder.ToString())
        {
            Content = new StringContent(request.Message.SerializeToJson())
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkMessageResponse>();
    }

    public async static Task<WeChatWorkResponse> SendMessageAsync(
        this HttpMessageInvoker client,
        WeChatWorkMessageRequest<WeChatWorkAppChatMessage> request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/appchat/send");
        urlBuilder.AppendFormat("?access_token={0}", request.AccessToken);

        using var httpRequest = new HttpRequestMessage(
            HttpMethod.Post,
            urlBuilder.ToString())
        {
            Content = new StringContent(request.Message.SerializeToJson())
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkResponse>();
    }

    public async static Task<WeChatWorkResponse> ReCallMessageAsync(
        this HttpMessageInvoker client,
        WeChatWorkMessageReCallRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/message/recall");
        urlBuilder.AppendFormat("?access_token={0}", request.AccessToken);

        using var httpRequest = new HttpRequestMessage(
            HttpMethod.Post,
            urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson())
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkResponse>();
    }

    public async static Task<WeChatWorkResponse> SendMessageAsync(
        this HttpMessageInvoker client,
        string webhookKey,
        WeChatWorkWebhookMessage request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/webhook/send");
        urlBuilder.AppendFormat("?key={0}", webhookKey);

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
