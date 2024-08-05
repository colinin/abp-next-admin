using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Webhooks
{
    public class DefaultWebhookSender : IWebhookSender, ITransientDependency
    {
        public ILogger<DefaultWebhookSender> Logger { protected get; set; }

        private readonly AbpWebhooksOptions _options;
        private readonly IWebhookManager _webhookManager;
        private readonly IHttpClientFactory _httpClientFactory;
        
        private const string FailedRequestDefaultContent = "Webhook Send Request Failed";

        public DefaultWebhookSender(
            IWebhookManager webhookManager,
            IHttpClientFactory httpClientFactory,
            IOptions<AbpWebhooksOptions> options)
        {
            _options = options.Value;
            _webhookManager = webhookManager;
            _httpClientFactory = httpClientFactory;

            Logger = NullLogger<DefaultWebhookSender>.Instance;
        }

        public async Task<Guid> SendWebhookAsync(WebhookSenderArgs webhookSenderArgs)
        {
            if (webhookSenderArgs.WebhookEventId == default)
            {
                throw new ArgumentNullException(nameof(webhookSenderArgs.WebhookEventId));
            }

            if (webhookSenderArgs.WebhookSubscriptionId == default)
            {
                throw new ArgumentNullException(nameof(webhookSenderArgs.WebhookSubscriptionId));
            }

            var webhookSendAttemptId = await _webhookManager.InsertAndGetIdWebhookSendAttemptAsync(webhookSenderArgs);

            var request = CreateWebhookRequestMessage(webhookSenderArgs);

            var serializedBody = await _webhookManager.GetSerializedBodyAsync(webhookSenderArgs);

            _webhookManager.SignWebhookRequest(request, serializedBody, webhookSenderArgs.Secret);

            AddAdditionalHeaders(request, webhookSenderArgs);

            var isSucceed = false;
            HttpStatusCode? statusCode = null;
            var content = FailedRequestDefaultContent;
            var reqHeaders = GetHeaders(request.Headers);
            IDictionary<string, string> resHeaders = null;

            try
            {
                var client = CreateWebhookClient(webhookSenderArgs);

                var response = await SendHttpRequest(client, request);

                isSucceed = response.IsSuccessStatusCode;
                statusCode = response.StatusCode;
                resHeaders = GetHeaders(response.Headers);
                content = await response.Content.ReadAsStringAsync();
            }
            catch (TaskCanceledException)
            {
                statusCode = HttpStatusCode.RequestTimeout;
                content = "Request Timeout";
            }
            catch (HttpRequestException e)
            {
                content = e.Message;
            }
            catch (Exception e)
            {
                Logger.LogError(e, "An error occured while sending a webhook request");
            }
            finally
            {
                await _webhookManager.StoreResponseOnWebhookSendAttemptAsync(
                    webhookSendAttemptId, 
                    webhookSenderArgs.TenantId, 
                    statusCode,
                    content,
                    reqHeaders,
                    resHeaders);
            }

            if (!isSucceed)
            {
                throw new Exception($"Webhook sending attempt failed. WebhookSendAttempt id: {webhookSendAttemptId}");
            }

            return webhookSendAttemptId;
        }

        /// <summary>
        /// You can override this to change request message
        /// </summary>
        /// <returns></returns>
        protected virtual HttpRequestMessage CreateWebhookRequestMessage(WebhookSenderArgs webhookSenderArgs)
        {
            return new HttpRequestMessage(HttpMethod.Post, webhookSenderArgs.WebhookUri);
        }

        protected virtual HttpClient CreateWebhookClient(WebhookSenderArgs webhookSenderArgs)
        {
            var client = _httpClientFactory.CreateClient(AbpWebhooksModule.WebhooksClient);
            if (webhookSenderArgs.TimeoutDuration.HasValue && 
                (webhookSenderArgs.TimeoutDuration >= 10 && webhookSenderArgs.TimeoutDuration <= 300))
            {
                client.Timeout = TimeSpan.FromSeconds(webhookSenderArgs.TimeoutDuration.Value);
            }

            return client;
        }

        protected virtual void AddAdditionalHeaders(HttpRequestMessage request, WebhookSenderArgs webhookSenderArgs)
        {
            foreach (var header in _options.DefaultHttpHeaders)
            {
                if (!request.Headers.Contains(header.Key))
                {
                    if (request.Headers.TryAddWithoutValidation(header.Key, header.Value))
                    {
                        continue;
                    }
                }

                if (!request.Content.Headers.Contains(header.Key))
                {
                    if (request.Content.Headers.TryAddWithoutValidation(header.Key, header.Value))
                    {
                        continue;
                    }
                }
            }

            foreach (var header in webhookSenderArgs.Headers)
            {
                if (request.Headers.Contains(header.Key) && request.Headers.Remove(header.Key))
                {
                    if (request.Headers.TryAddWithoutValidation(header.Key, header.Value))
                    {
                        continue;
                    }
                }
                else
                {
                    if (request.Headers.TryAddWithoutValidation(header.Key, header.Value))
                    {
                        continue;
                    }
                }

                if (request.Content.Headers.Contains(header.Key) && request.Content.Headers.Remove(header.Key))
                {
                    if (request.Content.Headers.TryAddWithoutValidation(header.Key, header.Value))
                    {
                        continue;
                    }
                }
                else
                {
                    if (request.Content.Headers.TryAddWithoutValidation(header.Key, header.Value))
                    {
                        continue;
                    }
                }

                Logger.LogWarning($"Invalid Header. SubscriptionId:{webhookSenderArgs.WebhookSubscriptionId},Header: {header.Key}:{header.Value}");
            }
        }

        protected async virtual Task<HttpResponseMessage> SendHttpRequest(HttpClient client, HttpRequestMessage request)
        {
            return await client.SendAsync(request);
        }

        private IDictionary<string, string> GetHeaders(HttpHeaders headers)
        {
            var res = new Dictionary<string, string>();

            if (headers != null && headers.Any())
            {
                foreach (var header in headers)
                {
                    res.Add(header.Key, header.Value.JoinAsString(";"));
                }
            }

            return res;
        }
    }
}
