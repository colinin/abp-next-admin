using LINGYUN.Abp.RealTime;
using Newtonsoft.Json;
using Shouldly;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Json;
using Xunit;

namespace LINGYUN.Abp.Notifications.Templating;
public class NotificationTemplateResolverTests : AbpNotificationsTemplatingTestBase
{
    private IJsonSerializer _jsonSerializer;
    public NotificationTemplateResolverTests()
    {
        _jsonSerializer = GetRequiredService<IJsonSerializer>();
    }

    [Fact]
    public async Task Resolve_Deserialize_To_Object()
    {
        var notificationTemplate = new NotificationTemplate(
            "Test",
            data: new Dictionary<string, object>
            {
                { "Name", "Tom" },
                { "Jobs", new List<NotificationJob>
                    {
                        new NotificationJob("Catch Jerry"),
                        new NotificationJob("Hit Pike")
                    } 
                }
            });

        var receivedEto = _jsonSerializer.Deserialize<RealTimeEto<NotificationTemplate>>(
            _jsonSerializer.Serialize(
                new RealTimeEto<NotificationTemplate>(notificationTemplate)));

        var contributor = new ToObjectNotificationTemplateResolveContributor();
        var context = new NotificationTemplateResolveContext(receivedEto.Data, ServiceProvider);
        
        await contributor.ResolveAsync(context);

        var model = context.Model.ShouldBeOfType<NotificationModel>();
        model.Name.ShouldBe("Tom");
        model.Jobs.Count.ShouldBe(2);
        model.Jobs[0].Name.ShouldBe("Catch Jerry");
        model.Jobs[1].Name.ShouldBe("Hit Pike");
    }

    [Fact]
    public async Task Resolve_Deserialize_To_Dynamic()
    {
        var notificationTemplate = new NotificationTemplate(
            "Test",
            data: new Dictionary<string, object>
            {
                { "Name", "Tom" },
                { "Firend", "Jerry" }
            });

        var receivedEto = _jsonSerializer.Deserialize<RealTimeEto<NotificationTemplate>>(
            _jsonSerializer.Serialize(
                new RealTimeEto<NotificationTemplate>(notificationTemplate)));

        var contributor = new NewtownsoftJsonTemplateResolveContributor();
        var context = new NotificationTemplateResolveContext(receivedEto.Data, ServiceProvider);

        await contributor.ResolveAsync(context);

        dynamic model = context.Model;

        Assert.Equal(model.Name, "Tom");
        Assert.Equal(model.Firend, "Jerry");
    }
}
