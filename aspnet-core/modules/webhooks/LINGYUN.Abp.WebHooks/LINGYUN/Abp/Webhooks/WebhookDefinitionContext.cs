using JetBrains.Annotations;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Webhooks
{
    public class WebhookDefinitionContext : IWebhookDefinitionContext
    {
        protected Dictionary<string, WebhookGroupDefinition> Groups { get; }

        public WebhookDefinitionContext(Dictionary<string, WebhookGroupDefinition> webhooks)
        {
            Groups = webhooks;
        }

        public WebhookGroupDefinition AddGroup(
            [NotNull] string name,
            ILocalizableString displayName = null)
        {
            Check.NotNull(name, nameof(name));

            if (Groups.ContainsKey(name))
            {
                throw new AbpException($"There is already an existing webhook group with name: {name}");
            }

            return Groups[name] = new WebhookGroupDefinition(name, displayName);
        }

        public WebhookGroupDefinition GetGroupOrNull([NotNull] string name)
        {
            Check.NotNull(name, nameof(name));

            if (!Groups.ContainsKey(name))
            {
                return null;
            }

            return Groups[name];
        }

        public void RemoveGroup(string name)
        {
            Check.NotNull(name, nameof(name));

            if (!Groups.ContainsKey(name))
            {
                throw new AbpException($"Undefined notification webhook group: '{name}'.");
            }

            Groups.Remove(name);
        }
    }
}
