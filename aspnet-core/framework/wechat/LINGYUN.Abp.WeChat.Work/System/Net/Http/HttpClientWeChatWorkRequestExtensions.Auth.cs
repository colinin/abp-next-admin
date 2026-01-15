using LINGYUN.Abp.WeChat.Work.Authorize.Request;
using LINGYUN.Abp.WeChat.Work.Authorize.Response;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

internal static partial class HttpClientWeChatWorkRequestExtensions
{
    public async static Task<WeChatWorkUserInfoResponse> GetUserInfoAsync(
        this HttpMessageInvoker client,
        string accessToken,
        string code,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/auth/getuserinfo");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);
        urlBuilder.AppendFormat("&code={0}", code);

        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, urlBuilder.ToString());

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkUserInfoResponse>();
    }

    public async static Task<WeChatWorkUserDetailResponse> GetUserDetailAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkUserDetailRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/auth/getuserdetail");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
            HttpMethod.Post,
            urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson())
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkUserDetailResponse>();
    }
}
