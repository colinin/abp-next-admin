using LINGYUN.Abp.WeChat.Work;
using LINGYUN.Abp.WeChat.Work.OA.Approvals.Request;
using LINGYUN.Abp.WeChat.Work.OA.Approvals.Response;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;
internal static partial class HttpClientWeChatWorkRequestExtensions
{
    public async static Task<WeChatWorkApplyEventResponse> ApplyEventAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkApplyEventRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/oa/applyevent");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson())
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkApplyEventResponse>();
    }

    public async static Task<WeChatWorkCreateTemplateResponse> CreateTemplateAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkCreateTemplateRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/oa/approval/create_template");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson())
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkCreateTemplateResponse>();
    }

    public async static Task<WeChatWorkGetApprovalDetailResponse> GetApprovalDetailAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkGetApprovalDetailRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/oa/getapprovaldetail");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson())
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkGetApprovalDetailResponse>();
    }

    public async static Task<WeChatWorkGetApprovalInfoResponse> GetApprovalInfoAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkGetApprovalInfoRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/oa/getapprovalinfo");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson())
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkGetApprovalInfoResponse>();
    }

    public async static Task<WeChatWorkTemplateResponse> GetTemplateAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkGetTemplateRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/oa/gettemplatedetail");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson())
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkTemplateResponse>();
    }

    public async static Task<WeChatWorkResponse> UpdateTemplateAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkCreateTemplateRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/oa/approval/update_template");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson())
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkTemplateResponse>();
    }
}
