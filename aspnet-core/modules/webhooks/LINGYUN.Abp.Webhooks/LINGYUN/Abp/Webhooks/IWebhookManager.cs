using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Webhooks
{
    public interface IWebhookManager
    {
        Task<WebhookPayload> GetWebhookPayloadAsync(WebhookSenderArgs webhookSenderArgs);

        void SignWebhookRequest(HttpRequestMessage request, string serializedBody, string secret);

        Task<string> GetSerializedBodyAsync(WebhookSenderArgs webhookSenderArgs);

        Task<Guid> InsertAndGetIdWebhookSendAttemptAsync(WebhookSenderArgs webhookSenderArgs);

        Task StoreResponseOnWebhookSendAttemptAsync(
            Guid webhookSendAttemptId, 
            Guid? tenantId,
            HttpStatusCode? statusCode, 
            string content,
            IDictionary<string, string> requestHeaders = null,
            IDictionary<string, string> responseHeaders = null);
    }
}
