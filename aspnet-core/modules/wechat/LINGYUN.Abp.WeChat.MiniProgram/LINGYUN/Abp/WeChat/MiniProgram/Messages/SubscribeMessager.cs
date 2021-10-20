using LINGYUN.Abp.Features.LimitValidation;
using LINGYUN.Abp.WeChat.MiniProgram.Features;
using LINGYUN.Abp.WeChat.OpenId;
using LINGYUN.Abp.WeChat.Token;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;

namespace LINGYUN.Abp.WeChat.MiniProgram.Messages
{
    [RequiresFeature(WeChatMiniProgramFeatures.Enable)]
    public class SubscribeMessager : ISubscribeMessager, ITransientDependency
    {
        public ILogger<SubscribeMessager> Logger { get; set; }
        protected IHttpClientFactory HttpClientFactory { get; }
        protected AbpWeChatMiniProgramOptionsFactory MiniProgramOptionsFactory { get; }
        protected IWeChatTokenProvider WeChatTokenProvider { get; }
        protected IUserWeChatOpenIdFinder UserWeChatOpenIdFinder { get; }
        public SubscribeMessager(
            IHttpClientFactory httpClientFactory,
            IWeChatTokenProvider weChatTokenProvider,
            IUserWeChatOpenIdFinder userWeChatOpenIdFinder,
            AbpWeChatMiniProgramOptionsFactory miniProgramOptionsFactory)
        {
            HttpClientFactory = httpClientFactory;
            WeChatTokenProvider = weChatTokenProvider;
            UserWeChatOpenIdFinder = userWeChatOpenIdFinder;
            MiniProgramOptionsFactory = miniProgramOptionsFactory;

            Logger = NullLogger<SubscribeMessager>.Instance;
        }

        [RequiresFeature(WeChatMiniProgramFeatures.Messages.Enable)]
        [RequiresLimitFeature(
            WeChatMiniProgramFeatures.Messages.SendLimit,
            WeChatMiniProgramFeatures.Messages.SendLimitInterval,
            LimitPolicy.Month,
            WeChatMiniProgramFeatures.Messages.DefaultSendLimit)]
        public virtual async Task SendAsync(
            Guid toUser,
            string templateId,
            string page = "",
            string lang = "zh_CN",
            string state = "formal",
            Dictionary<string, object> data = null,
            CancellationToken cancellation = default)
        {
            var openId = await UserWeChatOpenIdFinder.FindByUserIdAsync(toUser, AbpWeChatMiniProgramConsts.ProviderName);
            if (openId.IsNullOrWhiteSpace())
            {
                Logger.LogWarning("Can not found openId, Unable to send WeChat message!");
                return;
            }
            var messageData = new SubscribeMessage(openId, templateId, page, state, lang);
            if (data != null)
            {
                messageData.WriteData(data);
            }
            await SendAsync(messageData, cancellation);
        }

        [RequiresFeature(WeChatMiniProgramFeatures.Messages.Enable)]
        [RequiresLimitFeature(
            WeChatMiniProgramFeatures.Messages.SendLimit,
            WeChatMiniProgramFeatures.Messages.SendLimitInterval,
            LimitPolicy.Month,
            WeChatMiniProgramFeatures.Messages.DefaultSendLimit)]
        public virtual async Task SendAsync(SubscribeMessage message, CancellationToken cancellationToken = default)
        {
            var options = await MiniProgramOptionsFactory.CreateAsync();

            var weChatToken = await WeChatTokenProvider.GetTokenAsync(options.AppId, options.AppSecret, cancellationToken);
            var requestParamters = new Dictionary<string, string>
            {
                { "access_token", weChatToken.AccessToken }
            };
            var weChatSendNotificationUrl = "https://api.weixin.qq.com";
            var weChatSendNotificationPath = "/cgi-bin/message/subscribe/send";
            var requestUrl = BuildRequestUrl(weChatSendNotificationUrl, weChatSendNotificationPath, requestParamters);
            var responseContent = await MakeRequestAndGetResultAsync(requestUrl, message, cancellationToken);
            var response = JsonConvert.DeserializeObject<SubscribeMessageResponse>(responseContent);

            if (!response.IsSuccessed)
            {
                Logger.LogWarning("Send wechat we app subscribe message failed");
                Logger.LogWarning($"Error code: {response.ErrorCode}, message: {response.ErrorMessage}");
            }
        }

        protected virtual async Task<string> MakeRequestAndGetResultAsync(string url, SubscribeMessage message, CancellationToken cancellationToken = default)
        {
            var client = HttpClientFactory.CreateClient(AbpWeChatMiniProgramConsts.HttpClient);
            var sendDataContent = JsonConvert.SerializeObject(message);
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
}
