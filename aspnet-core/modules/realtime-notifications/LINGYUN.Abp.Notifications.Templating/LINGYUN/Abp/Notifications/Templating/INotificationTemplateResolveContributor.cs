using System.Threading.Tasks;

namespace LINGYUN.Abp.Notifications.Templating;
public interface INotificationTemplateResolveContributor
{
    string Name { get; }

    Task ResolveAsync(INotificationTemplateResolveContext context);
}
