using System.Net.Http;
using System.Threading.Tasks;

namespace OpenApi
{
    public interface IClientProxy
    {
        Task<ApiResponse<TResult>> GetAsync<TResult>(string url, string appKey, string appSecret);

        Task<ApiResponse<TResult>> DeleteAsync<TResult>(string url, string appKey, string appSecret);

        Task<ApiResponse<TResult>> PutAsync<TRequest, TResult>(string url, string appKey, string appSecret, TRequest request);

        Task<ApiResponse<TResult>> PostAsync<TRequest, TResult>(string url, string appKey, string appSecret, TRequest request);

        Task<ApiResponse<TResult>> RequestAsync<TResult>(string url, string appKey, string appSecret, HttpMethod httpMethod);

        Task<ApiResponse<TResult>> RequestAsync<TRequest, TResult>(string url, string appKey, string appSecret, TRequest request, HttpMethod httpMethod);

        Task<ApiResponse<TResult>> RequestAsync<TRequest, TResult>(HttpClient client, string url, string appKey, string appSecret, TRequest request, HttpMethod httpMethod);
    }
}
