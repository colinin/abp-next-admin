using System;
using System.Collections.Generic;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Webhooks
{
    public class WebhookDefinition
    {
        /// <summary>
        /// Unique name of the webhook.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Tries to send webhook only one time without checking to send attempt count
        /// </summary>
        public bool TryOnce { get; set; }

        /// <summary>
        /// Defined maximum number of sending times
        /// </summary>
        public int MaxSendAttemptCount { get; set; }

        /// <summary>
        /// Display name of the webhook.
        /// Optional.
        /// </summary>
        public ILocalizableString DisplayName { get; set; }

        /// <summary>
        /// Description for the webhook.
        /// Optional.
        /// </summary>
        public ILocalizableString Description { get; set; }

        public List<string> RequiredFeatures { get; set; }

        public WebhookDefinition(string name, ILocalizableString displayName = null, ILocalizableString description = null)
        {
            if (name.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(name), $"{nameof(name)} can not be null, empty or whitespace!");
            }

            Name = name.Trim();
            DisplayName = displayName;
            Description = description;

            RequiredFeatures = new List<string>();
        }

        public WebhookDefinition WithFeature(params string[] features)
        {
            if (!features.IsNullOrEmpty())
            {
                RequiredFeatures.AddRange(features);
            }

            return this;
        }

        public override string ToString()
        {
            return $"[{nameof(WebhookDefinition)} {Name}]";
        }
    }
}
