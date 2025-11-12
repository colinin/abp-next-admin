using LINGYUN.Abp.WeChat.Work.ExternalContact.Tags.Request;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;
internal static partial class HttpClientWeChatWorkRequestExtensions
{
    public async static Task<HttpResponseMessage> GetCropTagListAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkGetCropTagListRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/get_corp_tag_list");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson()),
        };

        return await client.SendAsync(httpRequest, cancellationToken);
    }

    public async static Task<HttpResponseMessage> CreateCropTagAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkCreateCropTagRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/add_corp_tag");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson()),
        };

        return await client.SendAsync(httpRequest, cancellationToken);
    }

    public async static Task<HttpResponseMessage> UpdateCropTagAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkUpdateCropTagRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/edit_corp_tag");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson()),
        };

        return await client.SendAsync(httpRequest, cancellationToken);
    }

    public async static Task<HttpResponseMessage> DeleteCropTagAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkDeleteCropTagRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/del_corp_tag");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson()),
        };

        return await client.SendAsync(httpRequest, cancellationToken);
    }

    public async static Task<HttpResponseMessage> MarkCropTagAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkMarkCropTagRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/mark_tag");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson()),
        };

        return await client.SendAsync(httpRequest, cancellationToken);
    }

    public async static Task<HttpResponseMessage> GetStrategyTagListAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkGetStrategyTagListRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/get_strategy_tag_list");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson()),
        };

        return await client.SendAsync(httpRequest, cancellationToken);
    }

    public async static Task<HttpResponseMessage> CreateStrategyTagAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkCreateStrategyTagRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/add_strategy_tag");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson()),
        };

        return await client.SendAsync(httpRequest, cancellationToken);
    }

    public async static Task<HttpResponseMessage> UpdateStrategyTagAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkUpdateStrategyTagRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/edit_strategy_tag");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson()),
        };

        return await client.SendAsync(httpRequest, cancellationToken);
    }

    public async static Task<HttpResponseMessage> DeleteStrategyTagAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkDeleteStrategyTagRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/externalcontact/del_strategy_tag");
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
