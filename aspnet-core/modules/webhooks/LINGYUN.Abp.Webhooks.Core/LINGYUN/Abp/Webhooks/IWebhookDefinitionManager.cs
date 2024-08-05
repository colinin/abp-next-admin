using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Webhooks;

public interface IWebhookDefinitionManager
{
    /// <summary>
    /// Gets a webhook definition by name.
    /// Returns null if there is no webhook definition with given name.
    /// </summary>
    Task<WebhookDefinition> GetOrNullAsync(string name);

    /// <summary>
    /// Gets a webhook definition by name.
    /// Throws exception if there is no webhook definition with given name.
    /// </summary>
    [NotNull] 
    Task<WebhookDefinition> GetAsync(string name);

    /// <summary>
    /// Gets all webhook definitions.
    /// </summary>
    Task<IReadOnlyList<WebhookDefinition>> GetWebhooksAsync();

    /// <summary>
    /// Gets a webhook group definition by name.
    /// Returns null if there is no webhook group definition with given name.
    /// </summary>
    Task<WebhookGroupDefinition> GetGroupOrNullAsync(string name);

    /// <summary>
    /// Gets a webhook definition by name.
    /// Throws exception if there is no webhook group definition with given name.
    /// </summary>
    [NotNull]
    Task<WebhookGroupDefinition> GetGroupAsync(string name);

    /// <summary>
    /// Gets all webhook group definitions.
    /// </summary>
    /// <returns></returns>
    Task<IReadOnlyList<WebhookGroupDefinition>> GetGroupsAsync();

    /// <summary>
    /// Checks if given webhook name is available for given tenant.
    /// </summary>
    Task<bool> IsAvailableAsync(Guid? tenantId, string name);
}
