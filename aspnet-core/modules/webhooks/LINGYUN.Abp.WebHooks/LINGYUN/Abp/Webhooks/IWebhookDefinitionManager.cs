using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Webhooks
{
    public interface IWebhookDefinitionManager
    {
        /// <summary>
        /// Gets a webhook definition by name.
        /// Returns null if there is no webhook definition with given name.
        /// </summary>
        WebhookDefinition GetOrNull(string name);

        /// <summary>
        /// Gets a webhook definition by name.
        /// Throws exception if there is no webhook definition with given name.
        /// </summary>
        WebhookDefinition Get(string name);

        /// <summary>
        /// Gets all webhook definitions.
        /// </summary>
        IReadOnlyList<WebhookDefinition> GetAll();

        /// <summary>
        /// Gets all webhook group definitions.
        /// </summary>
        /// <returns></returns>
        IReadOnlyList<WebhookGroupDefinition> GetGroups();

        /// <summary>
        /// Checks if given webhook name is available for given tenant.
        /// </summary>
        Task<bool> IsAvailableAsync(Guid? tenantId, string name);
    }
}
