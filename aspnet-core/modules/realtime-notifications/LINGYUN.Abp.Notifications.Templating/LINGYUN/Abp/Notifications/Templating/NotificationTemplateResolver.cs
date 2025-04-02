using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Notifications.Templating;
public class NotificationTemplateResolver : INotificationTemplateResolver, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;
    private readonly AbpNotificationsResolveOptions _options;

    public NotificationTemplateResolver(
        IOptions<AbpNotificationsResolveOptions> options,
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _options = options.Value;
    }
    public async virtual Task<NotificationTemplateResolveResult> ResolveAsync(NotificationTemplate template)
    {
        var result = new NotificationTemplateResolveResult();

        using (var serviceScope = _serviceProvider.CreateScope())
        {
            var context = new NotificationTemplateResolveContext(template, serviceScope.ServiceProvider);

            foreach (var resolveContributor in _options.TemplateResolvers)
            {
                // TODO: 设定为每一个通知都配置自己的解析提供者?
                /**
                   if (resolveContributor.Name.Equals(template.Name))
                   {
                      await resolveContributor.ResolveAsync(context);
                   }
                  
                   if (context.HasResolvedModel())
                   {
                        result.Model = context.Model;
                        break;
                   }
                 **/

                await resolveContributor.ResolveAsync(context);

                result.AppliedResolvers.Add(resolveContributor.Name);

                if (context.HasResolvedModel())
                {
                    result.Model = context.Model;
                    break;
                }
            }
        }

        return result;
    }
}
