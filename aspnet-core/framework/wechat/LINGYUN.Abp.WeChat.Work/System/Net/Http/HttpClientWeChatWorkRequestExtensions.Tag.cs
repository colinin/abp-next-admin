using LINGYUN.Abp.WeChat.Work.Tags.Request;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;
internal static partial class HttpClientWeChatWorkRequestExtensions
{
    public async static Task<HttpResponseMessage> AddTagMemberAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkTagChangeMemberRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/tag/addtagusers");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson())
        };

        return await client.SendAsync(httpRequest, cancellationToken);
    }
    public async static Task<HttpResponseMessage> CreateTagAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkTagCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/tag/create");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson())
        };

        return await client.SendAsync(httpRequest, cancellationToken);
    }
    public async static Task<HttpResponseMessage> DeleteTagAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkGetTagRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/tag/delete");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);
        urlBuilder.AppendFormat("&tagid={0}", request.TagId);

        var httpRequest = new HttpRequestMessage(
           HttpMethod.Get,
           urlBuilder.ToString());

        return await client.SendAsync(httpRequest, cancellationToken);
    }
    public async static Task<HttpResponseMessage> DeleteTagMemberAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkTagChangeMemberRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/tag/deltagusers");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson())
        };

        return await client.SendAsync(httpRequest, cancellationToken);
    }
    public async static Task<HttpResponseMessage> GetTagListAsync(
        this HttpMessageInvoker client,
        string accessToken,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/tag/list");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        var httpRequest = new HttpRequestMessage(
           HttpMethod.Get,
           urlBuilder.ToString());

        return await client.SendAsync(httpRequest, cancellationToken);
    }
    public async static Task<HttpResponseMessage> GetTagMemberAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkGetTagRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/tag/get");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);
        urlBuilder.AppendFormat("&tagid={0}", request.TagId);

        var httpRequest = new HttpRequestMessage(
           HttpMethod.Get,
           urlBuilder.ToString());

        return await client.SendAsync(httpRequest, cancellationToken);
    }
    public async static Task<HttpResponseMessage> UpdateTagAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkTagUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/tag/update");
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
