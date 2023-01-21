using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Http;

namespace LINGYUN.Abp.Webhooks
{
    public abstract class WebhookManager : IWebhookManager
    {
        private const string SignatureHeaderKey = "sha256";
        private const string SignatureHeaderValueTemplate = SignatureHeaderKey + "={0}";
        private const string SignatureHeaderName = "abp-webhook-signature";
        protected IWebhookSendAttemptStore WebhookSendAttemptStore { get; }

        protected WebhookManager(
            IWebhookSendAttemptStore webhookSendAttemptStore)
        {
            WebhookSendAttemptStore = webhookSendAttemptStore;
        }

        public async virtual Task<WebhookPayload> GetWebhookPayloadAsync(WebhookSenderArgs webhookSenderArgs)
        {
            var data = JsonConvert.DeserializeObject(webhookSenderArgs.Data);

            var attemptNumber = await WebhookSendAttemptStore.GetSendAttemptCountAsync(
                webhookSenderArgs.TenantId,
                webhookSenderArgs.WebhookEventId,
                webhookSenderArgs.WebhookSubscriptionId);

            return new WebhookPayload(
                webhookSenderArgs.WebhookEventId.ToString(),
                webhookSenderArgs.WebhookName,
                attemptNumber)
            {
                Data = data
            };
        }

        public virtual void SignWebhookRequest(HttpRequestMessage request, string serializedBody, string secret)
        {
            Check.NotNull(request, nameof(request));
            Check.NotNullOrWhiteSpace(serializedBody, nameof(serializedBody));

            request.Content = new StringContent(serializedBody, Encoding.UTF8, MimeTypes.Application.Json);

            if (!secret.IsNullOrWhiteSpace())
            {
                var secretBytes = Encoding.UTF8.GetBytes(secret);
                var headerValue = string.Format(CultureInfo.InvariantCulture, SignatureHeaderValueTemplate, serializedBody.Sha256(secretBytes));

                request.Headers.Add(SignatureHeaderName, headerValue);
            }
        }

        public async virtual Task<string> GetSerializedBodyAsync(WebhookSenderArgs webhookSenderArgs)
        {
            if (webhookSenderArgs.SendExactSameData)
            {
                return webhookSenderArgs.Data;
            }

            var payload = await GetWebhookPayloadAsync(webhookSenderArgs);

            var serializedBody = JsonConvert.SerializeObject(payload);

            return serializedBody;
        }

        public abstract Task<Guid> InsertAndGetIdWebhookSendAttemptAsync(WebhookSenderArgs webhookSenderArgs);

        public abstract Task StoreResponseOnWebhookSendAttemptAsync(
            Guid webhookSendAttemptId, 
            Guid? tenantId, 
            HttpStatusCode? statusCode, 
            string content,
            IDictionary<string, string> requestHeaders = null,
            IDictionary<string, string> responseHeaders = null);
    }
}
