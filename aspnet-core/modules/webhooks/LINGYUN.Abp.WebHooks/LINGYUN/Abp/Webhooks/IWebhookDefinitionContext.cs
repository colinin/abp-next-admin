namespace LINGYUN.Abp.Webhooks
{
    public interface IWebhookDefinitionContext
    {
        /// <summary>
        /// Adds the specified webhook definition. Throws exception if it is already added
        /// </summary>
        void Add(params WebhookDefinition[] definitions);

        /// <summary>
        /// Gets a webhook definition by name.
        /// Returns null if there is no webhook definition with given name.
        /// </summary>
        WebhookDefinition GetOrNull(string name);

        /// <summary>
        /// Remove webhook with given name
        /// </summary>
        /// <param name="name">webhook definition name</param>
        void Remove(string name);
    }
}
