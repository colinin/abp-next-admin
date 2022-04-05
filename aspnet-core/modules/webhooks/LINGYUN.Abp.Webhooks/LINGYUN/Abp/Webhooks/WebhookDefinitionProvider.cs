using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Webhooks
{
    public abstract class WebhookDefinitionProvider : ITransientDependency
    {
        /// <summary>
        /// Used to add/manipulate webhook definitions.
        /// </summary>
        /// <param name="context">Context</param>,
        public abstract void Define(IWebhookDefinitionContext context);
    }
}
