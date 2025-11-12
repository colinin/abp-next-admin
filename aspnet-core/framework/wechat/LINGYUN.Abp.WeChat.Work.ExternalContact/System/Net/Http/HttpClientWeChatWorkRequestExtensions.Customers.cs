using LINGYUN.Abp.WeChat.Work.ExternalContact.Customers.Request;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;
internal static partial class HttpClientWeChatWorkRequestExtensions
{
    public async static Task<HttpResponseMessage> GetCustomerListAsync(
        this HttpMessageInvoker client,
        string accessToken,
        string userId,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/list");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);
        urlBuilder.AppendFormat("&userid={0}", userId);

        var httpRequest = new HttpRequestMessage(
           HttpMethod.Get,
           urlBuilder.ToString());

        return await client.SendAsync(httpRequest, cancellationToken);
    }

    public async static Task<HttpResponseMessage> BulkGetCustomerAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkBulkGetCustomerRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/batch/get_by_user");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson())
        };

        return await client.SendAsync(httpRequest, cancellationToken);
    }

    public async static Task<HttpResponseMessage> GetCustomerAsync(
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

        var httpRequest = new HttpRequestMessage(
           HttpMethod.Get,
           urlBuilder.ToString());

        return await client.SendAsync(httpRequest, cancellationToken);
    }

    public async static Task<HttpResponseMessage> UpdateCustomerRemarkAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkUpdateCustomerRemarkRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/remark");
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
