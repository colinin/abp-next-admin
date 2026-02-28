using LINGYUN.Abp.WeChat.Work.ExternalContact.Transfers.Request;
using LINGYUN.Abp.WeChat.Work.ExternalContact.Transfers.Response;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;
internal static partial class HttpClientWeChatWorkRequestExtensions
{
    public async static Task<WeChatWorkTransferCustomerResponse> AssignCustomerAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkTransferCustomerRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/transfer_customer");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson()),
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkTransferCustomerResponse>();
    }

    public async static Task<WeChatWorkGetTransferResultResponse> GetTransferResultAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkGetTransferResultRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/transfer_result");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson()),
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkGetTransferResultResponse>();
    }

    public async static Task<WeChatWorkGroupChatOnjobTransferResponse> GroupChatOnjobTransferAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkGroupChatOnjobTransferRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/groupchat/onjob_transfer");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson()),
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkGroupChatOnjobTransferResponse>();
    }

    public async static Task<WeChatWorkGetUnassignedListResponse> GetUnassignedListAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkGetUnassignedListRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/get_unassigned_list");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson()),
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkGetUnassignedListResponse>();
    }

    public async static Task<WeChatWorkResignedTransferCustomerResponse> ResignedTransferCustomerAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkResignedTransferCustomerRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/resigned/transfer_customer");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson()),
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkResignedTransferCustomerResponse>();
    }

    public async static Task<WeChatWorkGetResignedTransferResultResponse> GetResignedTransferResultAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkGetResignedTransferResultRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/resigned/transfer_result");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson()),
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkGetResignedTransferResultResponse>();
    }

    public async static Task<WeChatWorkGroupChatTransferResponse> GroupChatTransferAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkGroupChatTransferRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/groupchat/transfer");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson()),
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkGroupChatTransferResponse>();
    }
}
