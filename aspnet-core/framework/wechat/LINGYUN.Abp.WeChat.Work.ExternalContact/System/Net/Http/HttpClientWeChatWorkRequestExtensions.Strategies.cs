using LINGYUN.Abp.WeChat.Work.ExternalContact.Customers.Request;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;
internal static partial class HttpClientWeChatWorkRequestExtensions
{
    public async static Task<HttpResponseMessage> GetCustomerStrategyListAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkGetCustomerStrategyListRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/customer_strategy/list");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson()),
        };

        return await client.SendAsync(httpRequest, cancellationToken);
    }

    public async static Task<HttpResponseMessage> GetCustomerStrategyAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkGetCustomerStrategyRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/customer_strategy/get");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson()),
        };

        return await client.SendAsync(httpRequest, cancellationToken);
    }

    public async static Task<HttpResponseMessage> GetCustomerStrategyRangeAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkGetCustomerStrategyRangeRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/customer_strategy/get_range");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson()),
        };

        return await client.SendAsync(httpRequest, cancellationToken);
    }

    public async static Task<HttpResponseMessage> CreateCustomerStrategyAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkCreateCustomerStrategyRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/customer_strategy/create");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson()),
        };

        return await client.SendAsync(httpRequest, cancellationToken);
    }

    public async static Task<HttpResponseMessage> UpdateCustomerStrategyAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkUpdateCustomerStrategyRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/customer_strategy/edit");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson()),
        };

        return await client.SendAsync(httpRequest, cancellationToken);
    }

    public async static Task<HttpResponseMessage> DeleteCustomerStrategyAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkDeleteCustomerStrategyRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/customer_strategy/del");
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
