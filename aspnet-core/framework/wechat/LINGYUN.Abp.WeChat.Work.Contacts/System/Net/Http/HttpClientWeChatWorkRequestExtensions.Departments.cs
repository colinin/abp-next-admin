using LINGYUN.Abp.WeChat.Work;
using LINGYUN.Abp.WeChat.Work.Contacts.Departments.Request;
using LINGYUN.Abp.WeChat.Work.Contacts.Departments.Response;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

internal static partial class HttpClientWeChatWorkRequestExtensions
{
    public async static Task<WeChatWorkGetDepartmentListResponse> GetDepartmentListAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkGetDepartmentListRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/department/list");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);
        if (request.Id.HasValue)
        {
            urlBuilder.AppendFormat("&id={0}", request.Id.Value);
        }

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Get,
           urlBuilder.ToString());

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkGetDepartmentListResponse>();
    }

    public async static Task<WeChatWorkGetSubDepartmentListResponse> GetSubDepartmentListAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkGetSubDepartmentListRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/department/simplelist");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);
        if (request.Id.HasValue)
        {
            urlBuilder.AppendFormat("&id={0}", request.Id.Value);
        }

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Get,
           urlBuilder.ToString());

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkGetSubDepartmentListResponse>();
    }

    public async static Task<WeChatWorkGetDepartmentResponse> GetDepartmentAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkGetDepartmentRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/department/get");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);
        urlBuilder.AppendFormat("&id={0}", request.Id);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Get,
           urlBuilder.ToString());

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkGetDepartmentResponse>();
    }

    public async static Task<WeChatWorkCreateDepartmentResponse> CreateDepartmentAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkCreateDepartmentRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/department/create");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson()),
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkCreateDepartmentResponse>();
    }

    public async static Task<WeChatWorkResponse> UpdateDepartmentAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkUpdateDepartmentRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/department/update");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson()),
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkResponse>();
    }

    public async static Task<WeChatWorkResponse> DeleteDepartmentAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkDeleteDepartmentRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/department/delete");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);
        urlBuilder.AppendFormat("&id={0}", request.Id);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Get,
           urlBuilder.ToString());

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkResponse>();
    }
}
