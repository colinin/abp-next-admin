using JetBrains.Annotations;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Webhooks
{
    public class WebhookDefinitionContext : IWebhookDefinitionContext
    {
        protected Dictionary<string, WebhookDefinition> Webhooks { get; }

        public WebhookDefinitionContext(Dictionary<string, WebhookDefinition> webhooks)
        {
            Webhooks = webhooks;
        }

        public void Add(params WebhookDefinition[] definitions)
        {
            if (definitions.IsNullOrEmpty())
            {
                return;
            }

            foreach (var definition in definitions)
            {
                Webhooks[definition.Name] = definition;
            }
        }

        public WebhookDefinition GetOrNull([NotNull] string name)
        {
            Check.NotNull(name, nameof(name));

            if (!Webhooks.ContainsKey(name))
            {
                return null;
            }

            return Webhooks[name];
        }

        public void Remove(string name)
        {
            Check.NotNull(name, nameof(name));

            if (!Webhooks.ContainsKey(name))
            {
                throw new AbpException($"Undefined notification webhook: '{name}'.");
            }

            Webhooks.Remove(name);
        }
    }
}
