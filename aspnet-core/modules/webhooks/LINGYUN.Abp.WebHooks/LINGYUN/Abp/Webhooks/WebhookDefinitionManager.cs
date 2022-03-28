using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Webhooks
{
    internal class WebhookDefinitionManager : IWebhookDefinitionManager, ISingletonDependency
    {
        protected IDictionary<string, WebhookGroupDefinition> WebhookGroupDefinitions => _lazyWebhookGroupDefinitions.Value;
        private readonly Lazy<Dictionary<string, WebhookGroupDefinition>> _lazyWebhookGroupDefinitions;

        protected IDictionary<string, WebhookDefinition> WebhookDefinitions => _lazyWebhookDefinitions.Value;
        private readonly Lazy<Dictionary<string, WebhookDefinition>> _lazyWebhookDefinitions;

        private readonly IServiceProvider _serviceProvider;
        private readonly AbpWebhooksOptions _options;

        public WebhookDefinitionManager(
            IServiceProvider serviceProvider,
            IOptions<AbpWebhooksOptions> options)
        {
            _serviceProvider = serviceProvider;
            _options = options.Value;

            _lazyWebhookGroupDefinitions = new Lazy<Dictionary<string, WebhookGroupDefinition>>(CreateWebhookGroupDefinitions);
            _lazyWebhookDefinitions = new Lazy<Dictionary<string, WebhookDefinition>>(CreateWebhookDefinitions);
        }

        public WebhookDefinition GetOrNull(string name)
        {
            if (!WebhookDefinitions.ContainsKey(name))
            {
                return null;
            }

            return WebhookDefinitions[name];
        }

        public WebhookDefinition Get(string name)
        {
            if (!WebhookDefinitions.ContainsKey(name))
            {
                throw new KeyNotFoundException($"Webhook definitions does not contain a definition with the key \"{name}\".");
            }

            return WebhookDefinitions[name];
        }

        public IReadOnlyList<WebhookDefinition> GetAll()
        {
            return WebhookDefinitions.Values.ToImmutableList();
        }

        public IReadOnlyList<WebhookGroupDefinition> GetGroups()
        {
            return WebhookGroupDefinitions.Values.ToImmutableList();
        }

        public async Task<bool> IsAvailableAsync(Guid? tenantId, string name)
        {
            if (tenantId == null) // host allowed to subscribe all webhooks
            {
                return true;
            }

            var webhookDefinition = GetOrNull(name);

            if (webhookDefinition == null)
            {
                return false;
            }

            if (webhookDefinition.RequiredFeatures?.Any() == false)
            {
                return true;
            }

            var currentTenant = _serviceProvider.GetRequiredService<ICurrentTenant>();
            var featureChecker = _serviceProvider.GetRequiredService<IFeatureChecker>();
            using (currentTenant.Change(tenantId))
            {
                if (!await featureChecker.IsEnabledAsync(true, webhookDefinition.RequiredFeatures.ToArray()))
                {
                    return false;
                }
            }

            return true;
        }

        protected virtual Dictionary<string, WebhookDefinition> CreateWebhookDefinitions()
        {
            var definitions = new Dictionary<string, WebhookDefinition>();

            foreach (var groupDefinition in WebhookGroupDefinitions.Values)
            {
                foreach (var webhook in groupDefinition.Webhooks)
                {
                    if (definitions.ContainsKey(webhook.Name))
                    {
                        throw new AbpException("Duplicate webhook name: " + webhook.Name);
                    }

                    definitions[webhook.Name] = webhook;
                }
            }

            return definitions;
        }

        protected virtual Dictionary<string, WebhookGroupDefinition> CreateWebhookGroupDefinitions()
        {
            var definitions = new Dictionary<string, WebhookGroupDefinition>();

            using (var scope = _serviceProvider.CreateScope())
            {
                var providers = _options
                    .DefinitionProviders
                    .Select(p => scope.ServiceProvider.GetRequiredService(p) as WebhookDefinitionProvider)
                    .ToList();

                foreach (var provider in providers)
                {
                    provider.Define(new WebhookDefinitionContext(definitions));
                }
            }

            return definitions;
        }
    }
}
