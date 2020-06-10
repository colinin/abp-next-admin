using LINGYUN.Abp.WeChat.Authorization;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace LINGYUN.Abp.Notifications.WeChat.WeApp
{
    public class WeChatWeAppNotificationSender : IWeChatWeAppNotificationSender, ITransientDependency
    {
        public const string SendNotificationClientName = "WeChatWeAppSendNotificationClient";
        protected IHttpClientFactory HttpClientFactory { get; }
        protected IJsonSerializer JsonSerializer { get; }
        protected IWeChatTokenProvider WeChatTokenProvider { get; }
        public WeChatWeAppNotificationSender(
            IJsonSerializer jsonSerializer,
            IHttpClientFactory httpClientFactory,
            IWeChatTokenProvider weChatTokenProvider)
        {
            JsonSerializer = jsonSerializer;
            HttpClientFactory = httpClientFactory;
            WeChatTokenProvider = weChatTokenProvider;
        }

        public virtual async Task SendAsync(WeChatWeAppSendNotificationData notificationData)
        {
            var weChatToken = await WeChatTokenProvider.GetTokenAsync();
            var requestParamters = new Dictionary<string, string>
            {
                { "access_token", weChatToken.AccessToken }
            };
            var weChatSendNotificationUrl = "https://api.weixin.qq.com";
            var weChatSendNotificationPath = "/cgi-bin/message/subscribe/send";
            var requestUrl = BuildRequestUrl(weChatSendNotificationUrl, weChatSendNotificationPath, requestParamters);
            var responseContent = await MakeRequestAndGetResultAsync(requestUrl, notificationData);
            var weChatSenNotificationResponse = JsonSerializer.Deserialize<WeChatSendNotificationResponse>(responseContent);
            weChatSenNotificationResponse.ThrowIfNotSuccess();
        }
        protected virtual async Task<string> MakeRequestAndGetResultAsync(string url, WeChatWeAppSendNotificationData notificationData)
        {
            var client = HttpClientFactory.CreateClient(SendNotificationClientName);
            var sendDataContent = JsonSerializer.Serialize(notificationData);
            var requestContent = new StringContent(sendDataContent);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = requestContent
            };

            var response = await client.SendAsync(requestMessage);
            if (!response.IsSuccessStatusCode)
            {
                throw new AbpException($"Baidu http request service returns error! HttpStatusCode: {response.StatusCode}, ReasonPhrase: {response.ReasonPhrase}");
            }
            var resultContent = await response.Content.ReadAsStringAsync();

            return resultContent;
        }

        protected virtual string BuildRequestUrl(string uri, string path, IDictionary<string, string> paramters)
        {
            var requestUrlBuilder = new StringBuilder(128);
            requestUrlBuilder.Append(uri);
            requestUrlBuilder.Append(path).Append("?");
            foreach (var paramter in paramters)
            {
                requestUrlBuilder.AppendFormat("{0}={1}", paramter.Key, paramter.Value);
                requestUrlBuilder.Append("&");
            }
            requestUrlBuilder.Remove(requestUrlBuilder.Length - 1, 1);
            return requestUrlBuilder.ToString();
        }
    }

    public class WeChatSendNotificationResponse
    {
        [JsonProperty("errcode")]
        public int ErrorCode { get; set; }

        [JsonProperty("errmsg")]
        public string ErrorMessage { get; set; }

        public void ThrowIfNotSuccess()
        {
            if (ErrorCode != 0)
            {
                throw new AbpException($"Send wechat weapp notification error:{ErrorMessage}");
            }
        }
    }
}
