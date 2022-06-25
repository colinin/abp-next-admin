using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.Notifications.TextTemplating;

public class NotificationTemplateContentContributor : ITemplateContentContributor, ITransientDependency
{
    public async virtual Task<string> GetOrNullAsync(TemplateContentContributorContext context)
    {
        var store = context.ServiceProvider.GetRequiredService<INotificationTemplateStore>();

        return await store.GetOrNullAsync(context.TemplateDefinition.Name, context.Culture);
    }
}
