using LINGYUN.Abp.Features.LimitValidation;
using LINGYUN.Abp.Notifications.WeChat.WeApp.Features;
using LINGYUN.Abp.WeChat.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;
using Volo.Abp.Json;

namespace LINGYUN.Abp.Notifications.WeChat.WeApp
{
    public class WeChatWeAppNotificationSender : IWeChatWeAppNotificationSender, ITransientDependency
    {
        public const string SendNotificationClientName = "WeChatWeAppSendNotificationClient";
        public ILogger<WeChatWeAppNotificationSender> Logger { get; set; }
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

            Logger = NullLogger<WeChatWeAppNotificationSender>.Instance;
        }

        [RequiresLimitFeature( // 检查消息发布功能限制
            WeChatWeAppFeatures.Notifications.PublishLimit, 
            WeChatWeAppFeatures.Notifications.PublishLimitInterval,
            LimitPolicy.Month,
            WeChatWeAppFeatures.Notifications.DefaultPublishLimit,
            WeChatWeAppFeatures.Notifications.DefaultPublishLimitInterval
            )]
        public virtual async Task SendAsync(WeChatWeAppSendNotificationData notificationData, CancellationToken cancellationToken = default)
        {
            var weChatToken = await WeChatTokenProvider.GetTokenAsync();
            var requestParamters = new Dictionary<string, string>
            {
                { "access_token", weChatToken.AccessToken }
            };
            var weChatSendNotificationUrl = "https://api.weixin.qq.com";
            var weChatSendNotificationPath = "/cgi-bin/message/subscribe/send";
            var requestUrl = BuildRequestUrl(weChatSendNotificationUrl, weChatSendNotificationPath, requestParamters);
            var responseContent = await MakeRequestAndGetResultAsync(requestUrl, notificationData, cancellationToken);
            var weChatSenNotificationResponse = JsonSerializer.Deserialize<WeChatSendNotificationResponse>(responseContent);
            
            if (!weChatSenNotificationResponse.IsSuccessed)
            {
                Logger.LogWarning("Send wechat we app subscribe message failed");
                Logger.LogWarning($"Error code: {weChatSenNotificationResponse.ErrorCode}, message: {weChatSenNotificationResponse.ErrorMessage}");
            }
            // 失败是否抛出异常
            // weChatSenNotificationResponse.ThrowIfNotSuccess();
        }
        protected virtual async Task<string> MakeRequestAndGetResultAsync(string url, WeChatWeAppSendNotificationData notificationData, CancellationToken cancellationToken = default)
        {
            var client = HttpClientFactory.CreateClient(SendNotificationClientName);
            var sendDataContent = JsonSerializer.Serialize(notificationData);
            var requestContent = new StringContent(sendDataContent);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = requestContent
            };

            var response = await client.SendAsync(requestMessage, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                throw new AbpException($"WeChat send subscribe message http request service returns error! HttpStatusCode: {response.StatusCode}, ReasonPhrase: {response.ReasonPhrase}");
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

        public bool IsSuccessed => ErrorCode == 0;

        public void ThrowIfNotSuccess()
        {
            if (ErrorCode != 0)
            {
                throw new AbpException($"Send wechat weapp notification error:{ErrorMessage}");
            }
        }
    }
}
