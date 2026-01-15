using LINGYUN.Abp.WeChat.Work;
using LINGYUN.Abp.WeChat.Work.ExternalContact.Customers.Request;
using LINGYUN.Abp.WeChat.Work.ExternalContact.Customers.Response;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;
internal static partial class HttpClientWeChatWorkRequestExtensions
{
    public async static Task<WeChatWorkGetCustomerListResponse> GetCustomerListAsync(
        this HttpMessageInvoker client,
        string accessToken,
        string userId,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/list");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);
        urlBuilder.AppendFormat("&userid={0}", userId);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Get,
           urlBuilder.ToString());

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkGetCustomerListResponse>();
    }

    public async static Task<WeChatWorkBulkGetCustomerResponse> BulkGetCustomerAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkBulkGetCustomerRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/batch/get_by_user");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson())
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkBulkGetCustomerResponse>();
    }

    public async static Task<WeChatWorkGetCustomerResponse> GetCustomerAsync(
        this HttpMessageInvoker client,
        string accessToken,
        string externalUserid,
        string? cursor = null,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/get");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);
        urlBuilder.AppendFormat("&external_userid={0}", externalUserid);
        if (!cursor.IsNullOrWhiteSpace())
        {
            urlBuilder.AppendFormat("&cursor={0}", cursor);
        }

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Get,
           urlBuilder.ToString());

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkGetCustomerResponse>();
    }

    public async static Task<WeChatWorkResponse> UpdateCustomerRemarkAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkUpdateCustomerRemarkRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/remark");
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
