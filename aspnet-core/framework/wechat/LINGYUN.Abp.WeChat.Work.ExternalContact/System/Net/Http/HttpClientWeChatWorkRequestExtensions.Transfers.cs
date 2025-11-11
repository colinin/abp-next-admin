using LINGYUN.Abp.WeChat.Work.ExternalContact.Transfers.Request;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;
internal static partial class HttpClientWeChatWorkRequestExtensions
{
    public async static Task<HttpResponseMessage> AssignCustomerAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkTransferCustomerRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/transfer_customer");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson()),
        };

        return await client.SendAsync(httpRequest, cancellationToken);
    }

    public async static Task<HttpResponseMessage> GetTransferResultAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkGetTransferResultRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/transfer_result");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson()),
        };

        return await client.SendAsync(httpRequest, cancellationToken);
    }

    public async static Task<HttpResponseMessage> OnjobTransferAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkGroupChatOnjobTransferRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/groupchat/onjob_transfer");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson()),
        };

        return await client.SendAsync(httpRequest, cancellationToken);
    }

    public async static Task<HttpResponseMessage> GetUnassignedListAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkGetUnassignedListRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/get_unassigned_list");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson()),
        };

        return await client.SendAsync(httpRequest, cancellationToken);
    }

    public async static Task<HttpResponseMessage> ResignedTransferCustomerAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkResignedTransferCustomerRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/resigned/transfer_customer");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson()),
        };

        return await client.SendAsync(httpRequest, cancellationToken);
    }

    public async static Task<HttpResponseMessage> GetResignedTransferResultAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkGetResignedTransferResultRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/resigned/transfer_result");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson()),
        };

        return await client.SendAsync(httpRequest, cancellationToken);
    }

    public async static Task<HttpResponseMessage> GroupChatTransferAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkGroupChatTransferRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/groupchat/transfer");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson()),
        };

        return await client.SendAsync(httpRequest, cancellationToken);
    }
}
