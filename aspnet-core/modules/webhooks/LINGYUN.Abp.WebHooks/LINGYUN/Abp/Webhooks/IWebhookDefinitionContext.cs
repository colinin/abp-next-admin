using JetBrains.Annotations;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Webhooks
{
    public interface IWebhookDefinitionContext
    {
        WebhookGroupDefinition AddGroup(
            [NotNull] string name,
            ILocalizableString displayName = null);

        WebhookGroupDefinition GetGroupOrNull(string name);

        void RemoveGroup(string name);
    }
}
