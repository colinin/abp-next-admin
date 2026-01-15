using LINGYUN.Abp.WeChat.Work;
using LINGYUN.Abp.WeChat.Work.Contacts.Members.Models;
using LINGYUN.Abp.WeChat.Work.Contacts.Members.Request;
using LINGYUN.Abp.WeChat.Work.Contacts.Members.Response;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;
internal static partial class HttpClientWeChatWorkRequestExtensions
{
    public async static Task<WeChatWorkGetMemberResponse> GetMemberAsync(
        this HttpMessageInvoker client,
        string accessToken,
        string userId,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/user/get");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);
        urlBuilder.AppendFormat("&userid={0}", userId);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Get,
           urlBuilder.ToString());

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkGetMemberResponse>();
    }

    public async static Task<WeChatWorkCreateMemberResponse> CreateMemberAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkCreateMemberRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/user/create");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson()),
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkCreateMemberResponse>();
    }

    public async static Task<WeChatWorkResponse> UpdateMemberAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkUpdateMemberRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/user/update");
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

    public async static Task<WeChatWorkResponse> DeleteMemberAsync(
        this HttpMessageInvoker client,
        string accessToken,
        string userId,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/user/delete");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);
        urlBuilder.AppendFormat("&userid={0}", userId);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Get,
           urlBuilder.ToString());

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkResponse>();
    }

    public async static Task<WeChatWorkResponse> BulkDeleteMemberAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkBulkDeleteMemberRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/user/batchdelete");
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

    public async static Task<WeChatWorkGetSimpleMemberListResponse> GetSimpleMemberListAsync(
        this HttpMessageInvoker client,
        string accessToken,
        int departmentId,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/user/simplelist");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);
        urlBuilder.AppendFormat("&department_id={0}", departmentId);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Get,
           urlBuilder.ToString());

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkGetSimpleMemberListResponse>();
    }

    public async static Task<WeChatWorkGetMemberListResponse> GetMemberListAsync(
        this HttpMessageInvoker client,
        string accessToken,
        int departmentId,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/user/list");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);
        urlBuilder.AppendFormat("&department_id={0}", departmentId);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Get,
           urlBuilder.ToString());

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkGetMemberListResponse>();
    }

    public async static Task<WeChatWorkConvertToOpenIdResponse> ConvertToOpenIdAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkConvertToOpenIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/user/convert_to_openid");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson()),
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkConvertToOpenIdResponse>();
    }

    public async static Task<WeChatWorkConvertToUserIdResponse> ConvertToUserIdAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkConvertToUserIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/user/convert_to_userid");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson()),
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkConvertToUserIdResponse>();
    }

    public async static Task<WeChatWorkResponse> AuthSuccessAsync(
        this HttpMessageInvoker client,
        string accessToken,
        string userId,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/user/authsucc");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);
        urlBuilder.AppendFormat("&userid={0}", userId);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Get,
           urlBuilder.ToString());

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkResponse>();
    }

    public async static Task<WeChatWorkBulkInviteMemberResponse> BulkInviteMemberAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkBulkInviteMemberRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/batch/invite");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson()),
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkBulkInviteMemberResponse>();
    }

    public async static Task<WeChatWorkGetJoinQrCodeResponse> GetJoinQrCodeAsync(
        this HttpMessageInvoker client,
        string accessToken,
        QrCodeSizeType sizeType,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/corp/get_join_qrcode");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);
        urlBuilder.AppendFormat("&size_type={0}", (int)sizeType);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Get,
           urlBuilder.ToString());

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkGetJoinQrCodeResponse>();
    }

    public async static Task<WeChatWorkGetUserIdResponse> GetUserIdByMobileAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkGetUserIdByMobileRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/user/getuserid");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson()),
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkGetUserIdResponse>();
    }

    public async static Task<WeChatWorkGetUserIdResponse> GetUserIdByEmailAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkGetUserIdByEmailRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/user/get_userid_by_email");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson()),
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkGetUserIdResponse>();
    }

    public async static Task<WeChatWorkGetUserIdListResponse> GetUserIdListAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkGetUserIdListRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/user/list_id");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson()),
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkGetUserIdListResponse>();
    }
}
