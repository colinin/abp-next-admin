using System.Net.Http;
using System.Threading.Tasks;

namespace OpenApi
{
    public static class IClientProxyExtensions
    {
        public static async Task<ApiResponse<TResult>> RequestAsync<TResult>(
            this IClientProxy proxy,
            HttpClient client, 
            string url, 
            string appKey, 
            string appSecret, 
            HttpMethod httpMethod)
        {
            return await proxy.RequestAsync<object, TResult>(client, url, appKey, appSecret, null, httpMethod);
        }
    }
}
