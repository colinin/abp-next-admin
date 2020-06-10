using IdentityModel.Client;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http
{
    public static class HttpClientTokenRequestExtensions
    {
        public static async Task<WeChatOpenIdResponse> RequestWeChatCodeTokenAsync(this HttpMessageInvoker client, WeChatOpenIdRequest request, CancellationToken cancellationToken = default)
        {
            var getResuestUrlBuiilder = new StringBuilder();
            getResuestUrlBuiilder.Append(request.BaseUrl);
            getResuestUrlBuiilder.AppendFormat("?appid={0}", request.AppId);
            getResuestUrlBuiilder.AppendFormat("&secret={0}", request.Secret);
            getResuestUrlBuiilder.AppendFormat("&js_code={0}", request.Code);
            getResuestUrlBuiilder.Append("&grant_type=authorization_code");

            var getRequest = new HttpRequestMessage(HttpMethod.Get, getResuestUrlBuiilder.ToString());
            HttpResponseMessage httpResponse;
            try
            {
                httpResponse = await client.SendAsync(getRequest, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return ProtocolResponse.FromException<WeChatOpenIdResponse>(ex);
            }
            return await ProtocolResponse.FromHttpResponseAsync<WeChatOpenIdResponse>(httpResponse).ConfigureAwait(false);
        }
    }
}
