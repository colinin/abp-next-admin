using LINGYUN.Abp.Webhooks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.WebhooksManagement.Extensions
{
    public static class WebhookSubscriptionInfoExtensions
    {
        /// <summary>
        /// Return List of subscribed webhooks definitions <see cref="WebhookSubscriptionInfo.Webhooks"/>
        /// </summary>
        /// <returns></returns>
        public static List<string> GetSubscribedWebhooks(this WebhookSubscription webhookSubscription)
        {
            if (webhookSubscription.Webhooks.IsNullOrWhiteSpace())
            {
                return new List<string>();
            }

            return JsonConvert.DeserializeObject<List<string>>(webhookSubscription.Webhooks);
        }

        /// <summary>
        /// Adds webhook subscription to <see cref="WebhookSubscriptionInfo.Webhooks"/> if not exists
        /// </summary>
        /// <param name="webhookSubscription"></param>
        /// <param name="name">webhook unique name</param>
        public static void SubscribeWebhook(this WebhookSubscription webhookSubscription, string name)
        {
            name = name.Trim();
            if (name.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(name), $"{nameof(name)} can not be null, empty or whitespace!");
            }

            var webhookDefinitions = webhookSubscription.GetSubscribedWebhooks();
            if (webhookDefinitions.Contains(name))
            {
                return;
            }

            webhookDefinitions.Add(name);
            webhookSubscription.SetWebhooks(JsonConvert.SerializeObject(webhookDefinitions));
        }

        /// <summary>
        ///  Removes webhook subscription from <see cref="WebhookSubscriptionInfo.Webhooks"/> if exists
        /// </summary>
        /// <param name="webhookSubscription"></param>
        /// <param name="name">webhook unique name</param>
        public static void UnsubscribeWebhook(this WebhookSubscription webhookSubscription, string name)
        {
            name = name.Trim();
            if (name.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(name), $"{nameof(name)} can not be null, empty or whitespace!");
            }

            var webhookDefinitions = webhookSubscription.GetSubscribedWebhooks();
            if (!webhookDefinitions.Contains(name))
            {
                return;
            }

            webhookDefinitions.Remove(name);
            webhookSubscription.SetWebhooks(JsonConvert.SerializeObject(webhookDefinitions));
        }

        /// <summary>
        /// Clears all <see cref="WebhookSubscriptionInfo.Webhooks"/> 
        /// </summary>
        /// <param name="webhookSubscription"></param> 
        public static void RemoveAllSubscribedWebhooks(this WebhookSubscription webhookSubscription)
        {
            webhookSubscription.SetWebhooks(null);
        }

        /// <summary>
        /// if subscribed to given webhook
        /// </summary>
        /// <returns></returns>
        public static bool IsSubscribed(this WebhookSubscription webhookSubscription, string webhookName)
        {
            if (webhookSubscription.Webhooks.IsNullOrWhiteSpace())
            {
                return false;
            }

            return webhookSubscription.GetSubscribedWebhooks().Contains(webhookName);
        }

        /// <summary>
        /// Returns additional webhook headers <see cref="WebhookSubscriptionInfo.Headers"/>
        /// </summary>
        /// <returns></returns>
        public static IDictionary<string, string> GetWebhookHeaders(this WebhookSubscription webhookSubscription)
        {
            if (webhookSubscription.Headers.IsNullOrWhiteSpace())
            {
                return new Dictionary<string, string>();
            }

            return JsonConvert.DeserializeObject<Dictionary<string, string>>(webhookSubscription.Headers);
        }

        /// <summary>
        /// Adds webhook subscription to <see cref="WebhookSubscriptionInfo.Webhooks"/> if not exists
        /// </summary>
        public static void AddWebhookHeader(this WebhookSubscription webhookSubscription, string key, string value)
        {
            if (key.IsNullOrWhiteSpace() )
            {
                throw new ArgumentNullException(nameof(key), $"{nameof(key)} can not be null, empty or whitespace!");
            }

            if (value.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(value), $"{nameof(value)} can not be null, empty or whitespace!");
            }

            var headers = webhookSubscription.GetWebhookHeaders();
            headers[key] = value;

            webhookSubscription.SetHeaders(JsonConvert.SerializeObject(headers));
        }

        /// <summary>
        /// Adds webhook subscription to <see cref="WebhookSubscriptionInfo.Webhooks"/> if not exists
        /// </summary>
        /// <param name="webhookSubscription"></param>
        /// <param name="header">Key of header</param>
        public static void RemoveWebhookHeader(this WebhookSubscription webhookSubscription, string header)
        {
            if (header.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(header), $"{nameof(header)} can not be null, empty or whitespace!");
            }

            var headers = webhookSubscription.GetWebhookHeaders();

            if (!headers.ContainsKey(header))
            {
                return;
            }

            headers.Remove(header);

            webhookSubscription.SetHeaders(JsonConvert.SerializeObject(headers));
        }

        /// <summary>
        /// Clears all <see cref="WebhookSubscriptionInfo.Webhooks"/> 
        /// </summary>
        /// <param name="webhookSubscription"></param> 
        public static void RemoveAllWebhookHeaders(this WebhookSubscription webhookSubscription)
        {
            webhookSubscription.SetHeaders(null);
        }

        public static WebhookSubscriptionInfo ToWebhookSubscriptionInfo(this WebhookSubscription webhookSubscription)
        {
            return new WebhookSubscriptionInfo
            {
                Id = webhookSubscription.Id,
                TenantId = webhookSubscription.TenantId,
                IsActive = webhookSubscription.IsActive,
                Secret = webhookSubscription.Secret,
                WebhookUri = webhookSubscription.WebhookUri,
                Webhooks = webhookSubscription.GetSubscribedWebhooks(),
                Headers = webhookSubscription.GetWebhookHeaders()
            };
        }
    }
}
