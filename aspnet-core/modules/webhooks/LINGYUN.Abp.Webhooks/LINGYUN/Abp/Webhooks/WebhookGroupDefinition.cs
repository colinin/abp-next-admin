using JetBrains.Annotations;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Webhooks;

public class WebhookGroupDefinition
{
    [NotNull]
    public string Name { get; set; }
    public Dictionary<string, object> Properties { get; }

    private ILocalizableString _displayName;
    public ILocalizableString DisplayName 
    {
        get {
            return _displayName;
        }
        set {
            _displayName = value;
        }
    }

    public IReadOnlyList<WebhookDefinition> Webhooks => _webhooks.ToImmutableList();
    private readonly List<WebhookDefinition> _webhooks;
    public object this[string name] {
        get => Properties.GetOrDefault(name);
        set => Properties[name] = value;
    }

    protected internal WebhookGroupDefinition(
        string name,
        ILocalizableString displayName = null)
    {
        Name = name;
        DisplayName = displayName ?? new FixedLocalizableString(Name);

        Properties = new Dictionary<string, object>();
        _webhooks = new List<WebhookDefinition>();
    }

    public virtual WebhookDefinition AddWebhook(
        string name, 
        ILocalizableString displayName = null, 
        ILocalizableString description = null)
    {
        if (Webhooks.Any(hook => hook.Name.Equals(name)))
        {
            throw new AbpException($"There is already an existing webhook with name: {name} in group {Name}");
        }

        var webhook = new WebhookDefinition(
            name,
            displayName,
            description
        );

        _webhooks.Add(webhook);

        return webhook;
    }

    public virtual void AddWebhooks(params WebhookDefinition[] webhooks)
    {
        foreach (var webhook in webhooks)
        {
            if (Webhooks.Any(hook => hook.Name.Equals(webhook.Name)))
            {
                throw new AbpException($"There is already an existing webhook with name: {webhook.Name} in group {Name}");
            }
        }

        _webhooks.AddRange(webhooks);
    }



    [CanBeNull]
    public WebhookDefinition GetWebhookOrNull([NotNull] string name)
    {
        Check.NotNull(name, nameof(name));

        foreach (var webhook in Webhooks)
        {
            if (webhook.Name == name)
            {
                return webhook;
            }
        }

        return null;
    }

    public override string ToString()
    {
        return $"[{nameof(WebhookGroupDefinition)} {Name}]";
    }
}
