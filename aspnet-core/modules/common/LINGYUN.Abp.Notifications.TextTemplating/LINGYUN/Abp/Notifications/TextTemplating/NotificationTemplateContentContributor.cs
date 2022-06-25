using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.Notifications.TextTemplating;

public class NotificationTemplateContentContributor : ITemplateContentContributor, ITransientDependency
{
    public async virtual Task<string> GetOrNullAsync(TemplateContentContributorContext context)
    {
        var notificationDefinitionManager = context.ServiceProvider.GetRequiredService<INotificationDefinitionManager>();
        var notification = notificationDefinitionManager.GetOrNull(context.TemplateDefinition.Name);
        if (notification == null)
        {
            return null;
        }

        var store = context.ServiceProvider.GetRequiredService<INotificationTemplateStore>();

        return await store.GetContentOrNullAsync(context.TemplateDefinition.Name, context.Culture);
    }
}
