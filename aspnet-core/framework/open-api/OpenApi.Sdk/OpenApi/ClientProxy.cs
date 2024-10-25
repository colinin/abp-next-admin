using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OpenApi
{
    public class ClientProxy : IClientProxy
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ClientProxy(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async virtual Task<ApiResponse<TResult>> DeleteAsync<TResult>(string url, string appKey, string appSecret)
        {
            return await RequestAsync<TResult>(url, appKey, appSecret, HttpMethod.Delete);
        }

        public async virtual Task<ApiResponse<TResult>> GetAsync<TResult>(string url, string appKey, string appSecret)
        {
            return await RequestAsync<TResult>(url, appKey, appSecret, HttpMethod.Get);
        }

        public async virtual Task<ApiResponse<TResult>> PostAsync<TRequest, TResult>(string url, string appKey, string appSecret, TRequest request)
        {
            return await RequestAsync<TRequest, TResult>(url, appKey, appSecret, request, HttpMethod.Post);
        }

        public async virtual Task<ApiResponse<TResult>> PutAsync<TRequest, TResult>(string url, string appKey, string appSecret, TRequest request)
        {
            return await RequestAsync<TRequest, TResult>(url, appKey, appSecret, request, HttpMethod.Put);
        }

        public async virtual Task<ApiResponse<TResult>> RequestAsync<TResult>(string url, string appKey, string appSecret, HttpMethod httpMethod)
        {
            return await RequestAsync<object, TResult>(url, appKey, appSecret, null, httpMethod);
        }

        public async virtual Task<ApiResponse<TResult>> RequestAsync<TRequest, TResult>(string url, string appKey, string appSecret, TRequest request, HttpMethod httpMethod)
        {
            // 构建请求客户端
            var client = _httpClientFactory.CreateClient("opensdk");

            return await RequestAsync<TRequest, TResult>(client, url, appKey, appSecret, request, httpMethod);
        }

        public async virtual Task<ApiResponse<TResult>> RequestAsync<TRequest, TResult>(HttpClient client, string url, string appKey, string appSecret, TRequest request, HttpMethod httpMethod)
        {
            // UTC时间戳
            var timeStamp = GetUtcTimeStampString();
            // 随机数
            var nonce = Guid.NewGuid().ToString();
            // 取出api地址
            var baseUrl = url.Split('?')[0];
            // 组装请求参数
            var requestUrl = string.Concat(
                url,
                url.Contains('?') ? "&" : "?",
                "appKey=",
                appKey,
                "appSecret=",
                appSecret,
                "nonce=",
                nonce,
                "&t=",
                timeStamp);
            var queryString = ReverseQueryString(requestUrl);
            // 对请求参数签名
            var sign = CalculationSignature(baseUrl, queryString);

            // 构建请求体
            var requestMessage = new HttpRequestMessage(httpMethod, url);

            if (request != null)
            {
                // Request Payload
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(request));
            }
            // appKey添加到headers
            requestMessage.Headers.TryAddWithoutValidation("X-API-APPKEY", appKey);
            // 随机数添加到headers
            requestMessage.Headers.TryAddWithoutValidation("X-API-NONCE", nonce);
            // 时间戳添加到headers
            requestMessage.Headers.TryAddWithoutValidation("X-API-TIMESTAMP", timeStamp);
            // 签名添加到headers
            requestMessage.Headers.TryAddWithoutValidation("X-API-SIGN", sign);
            // 返回本地化错误提示
            requestMessage.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(CultureInfo.CurrentUICulture.Name));
            // 返回错误消息可序列化
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // 序列化响应
            var response = await client.SendAsync(requestMessage);

            var stringContent = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ApiResponse<TResult>>(stringContent);
        }

        protected virtual string GetUtcTimeStampString()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
        }

        private static IDictionary<string, string> ReverseQueryString(string requestUri)
        {
            if (!requestUri.Contains("?"))
            {
                throw new Exception("查询路径格式不合法");
            }

            var queryString = requestUri.Split('?')[1];
            // 按照首字母排序
            var paramters = queryString.Split('&').OrderBy(p => p);

            var queryDic = new Dictionary<string, string>();
            foreach (var parm in paramters)
            {
                var thisParams = parm.Split('=');
                if (thisParams.Length == 0)
                {
                    continue;
                }
                queryDic.Add(thisParams[0], thisParams.Length > 0 ? thisParams[1] : "");
            }

            // 返回参数列表
            return queryDic;
        }

        private static string CalculationSignature(string url, IDictionary<string, string> queryDictionary)
        {
            var queryString = BuildQuery(queryDictionary);
            var requestUrl = string.Concat(
                url,
                url.Contains('?') ? "" : "?",
                queryString);
            var encodeUrl = UrlEncode(requestUrl);
            return encodeUrl.ToMd5();
        }

        private static string BuildQuery(IDictionary<string, string> queryStringDictionary)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var queryString in queryStringDictionary.OrderBy(q => q.Key))
            {
                sb.Append(queryString.Key)
                  .Append('=')
                  .Append(queryString.Value)
                  .Append('&');
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        private static string UrlEncode(string str)
        {
            return HttpUtility.UrlEncode(str, Encoding.UTF8).ToUpper();
        }
    }
}
