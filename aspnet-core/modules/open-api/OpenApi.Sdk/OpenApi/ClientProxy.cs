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

        public virtual async Task<ApiResponse<TResult>> DeleteAsync<TResult>(string url, string appKey, string appSecret)
        {
            return await RequestAsync<TResult>(url, appKey, appSecret, HttpMethod.Delete);
        }

        public virtual async Task<ApiResponse<TResult>> GetAsync<TResult>(string url, string appKey, string appSecret)
        {
            return await RequestAsync<TResult>(url, appKey, appSecret, HttpMethod.Get);
        }

        public virtual async Task<ApiResponse<TResult>> PostAsync<TRequest, TResult>(string url, string appKey, string appSecret, TRequest request)
        {
            return await RequestAsync<TRequest, TResult>(url, appKey, appSecret, request, HttpMethod.Post);
        }

        public virtual async Task<ApiResponse<TResult>> PutAsync<TRequest, TResult>(string url, string appKey, string appSecret, TRequest request)
        {
            return await RequestAsync<TRequest, TResult>(url, appKey, appSecret, request, HttpMethod.Put);
        }

        public virtual async Task<ApiResponse<TResult>> RequestAsync<TResult>(string url, string appKey, string appSecret, HttpMethod httpMethod)
        {
            return await RequestAsync<object, TResult>(url, appKey, appSecret, null, httpMethod);
        }

        public virtual async Task<ApiResponse<TResult>> RequestAsync<TRequest, TResult>(string url, string appKey, string appSecret, TRequest request, HttpMethod httpMethod)
        {
            // 构建请求客户端
            var client = _httpClientFactory.CreateClient("opensdk");

            return await RequestAsync<TRequest, TResult>(client, url, appKey, appSecret, request, httpMethod);
        }

        public virtual async Task<ApiResponse<TResult>> RequestAsync<TRequest, TResult>(HttpClient client, string url, string appKey, string appSecret, TRequest request, HttpMethod httpMethod)
        {
            // UTC时间戳
            var timeStamp = GetUtcTimeStampString();
            // 取出api地址
            var baseUrl = url.Split('?')[0];
            // 组装请求参数
            var requestUrl = string.Concat(
                url,
                url.Contains('?') ? "&" : "?",
                "appKey=",
                appKey,
                "&t=",
                timeStamp);
            var quertString = ReverseQueryString(requestUrl);
            // 密钥参与计算
            quertString.Add("appSecret", appSecret);
            // 对请求参数签名
            var sign = CalculationSignature(baseUrl, quertString);
            // 移除密钥
            quertString.Remove("appSecret");
            // 签名随请求传递
            quertString.Add("sign", sign);
            // 重新拼接请求参数
            requestUrl = string.Concat(baseUrl, "?", BuildQuery(quertString));
            // 构建请求体
            var requestMessage = new HttpRequestMessage(httpMethod, requestUrl);

            if (request != null)
            {
                // Request Payload
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(request));
            }

            // 返回中文错误提示
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
