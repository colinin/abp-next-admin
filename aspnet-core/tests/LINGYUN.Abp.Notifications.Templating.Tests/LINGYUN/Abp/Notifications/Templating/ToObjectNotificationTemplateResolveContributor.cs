using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Json;

namespace LINGYUN.Abp.Notifications.Templating;
internal class ToObjectNotificationTemplateResolveContributor : INotificationTemplateResolveContributor
{
    public string Name => "ToObject";

    public Task ResolveAsync(INotificationTemplateResolveContext context)
    {
        var model = new NotificationModel();

        var nameObj = context.Template.GetProperty(nameof(NotificationModel.Name));
        model.Name = nameObj.ToString();

        var jobsObj = context.Template.GetProperty(nameof(NotificationModel.Jobs));

        var jsonSerializer = context.ServiceProvider.GetRequiredService<IJsonSerializer>();

        model.Jobs = jsonSerializer.Deserialize<List<NotificationJob>>(jobsObj.ToString());

        context.Model = model;

        return Task.CompletedTask;
    }
}
