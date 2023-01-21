using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Webhooks;

public class StaticWebhookDefinitionStore : IStaticWebhookDefinitionStore, ISingletonDependency
{
    protected IDictionary<string, WebhookGroupDefinition> WebhookGroupDefinitions => _lazyWebhookGroupDefinitions.Value;
    private readonly Lazy<Dictionary<string, WebhookGroupDefinition>> _lazyWebhookGroupDefinitions;

    protected IDictionary<string, WebhookDefinition> WebhookDefinitions => _lazyWebhookDefinitions.Value;
    private readonly Lazy<Dictionary<string, WebhookDefinition>> _lazyWebhookDefinitions;

    protected AbpWebhooksOptions Options { get; }

    private readonly IServiceScopeFactory _serviceScopeFactory;

    public StaticWebhookDefinitionStore(
        IServiceScopeFactory serviceScopeFactory,
        IOptions<AbpWebhooksOptions> options)
    {
        _serviceScopeFactory = serviceScopeFactory;
        Options = options.Value;

        _lazyWebhookDefinitions = new Lazy<Dictionary<string, WebhookDefinition>>(
            CreateWebhookDefinitions,
            isThreadSafe: true
        );

        _lazyWebhookGroupDefinitions = new Lazy<Dictionary<string, WebhookGroupDefinition>>(
            CreateWebhookGroupDefinitions,
            isThreadSafe: true
        );
    }

    protected virtual Dictionary<string, WebhookDefinition> CreateWebhookDefinitions()
    {
        var Webhooks = new Dictionary<string, WebhookDefinition>();

        foreach (var groupDefinition in WebhookGroupDefinitions.Values)
        {
            foreach (var Webhook in groupDefinition.Webhooks)
            {
                if (Webhooks.ContainsKey(Webhook.Name))
                {
                    throw new AbpException("Duplicate webhook name: " + Webhook.Name);
                }

                Webhooks[Webhook.Name] = Webhook;
            }
        }

        return Webhooks;
    }

    protected virtual Dictionary<string, WebhookGroupDefinition> CreateWebhookGroupDefinitions()
    {
        var definitions = new Dictionary<string, WebhookGroupDefinition>();

        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var providers = Options
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

    public Task<WebhookDefinition> GetOrNullAsync(string name)
    {
        return Task.FromResult(WebhookDefinitions.GetOrDefault(name));
    }

    public virtual Task<IReadOnlyList<WebhookDefinition>> GetWebhooksAsync()
    {
        return Task.FromResult<IReadOnlyList<WebhookDefinition>>(
            WebhookDefinitions.Values.ToImmutableList()
        );
    }

    public Task<IReadOnlyList<WebhookGroupDefinition>> GetGroupsAsync()
    {
        return Task.FromResult<IReadOnlyList<WebhookGroupDefinition>>(
            WebhookGroupDefinitions.Values.ToImmutableList()
        );
    }
}
