using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Notifications.Templating;
internal class NewtownsoftJsonTemplateResolveContributor : INotificationTemplateResolveContributor
{
    public string Name => "ToDynamic";

    public Task ResolveAsync(INotificationTemplateResolveContext context)
    {
        var jsonObject = new JObject();
        foreach (var prop in context.Template.ExtraProperties)
        {
            jsonObject.Add(prop.Key, prop.Value.ToString());
        }
        context.Model = jsonObject.ToObject<NotificationSimpleModel>();

        return Task.CompletedTask;
    }
}
