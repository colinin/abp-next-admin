using Elsa;
using Elsa.Options;
using LINGYUN.Abp.Elsa.Activities.BlobStoring;
using LINGYUN.Abp.Elsa.Activities.Emailing;
using LINGYUN.Abp.Elsa.Activities.IM;
using LINGYUN.Abp.Elsa.Activities.Notifications;
using LINGYUN.Abp.Elsa.Activities.Sms;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Elsa.Activities;

[DependsOn(
    typeof(AbpElsaModule),
    typeof(AbpElsaActivitiesBlobStoringModule),
    typeof(AbpElsaActivitiesEmailingModule),
    typeof(AbpElsaActivitiesIMModule),
    typeof(AbpElsaActivitiesNotificationsModule),
    typeof(AbpElsaActivitiesSmsModule))]
public class AbpElsaActivitiesModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var startups = new[]
        {
            typeof(Emailing.Startup),
            typeof(BlobStoring.Startup),
            typeof(Notifications.Startup),
            typeof(Sms.Startup),
            typeof(IM.Startup),
            typeof(Webhooks.Startup),
        };

        PreConfigure<ElsaOptionsBuilder>(elsa =>
        {
            elsa.AddFeatures(startups, configuration);
        });
    }
}
