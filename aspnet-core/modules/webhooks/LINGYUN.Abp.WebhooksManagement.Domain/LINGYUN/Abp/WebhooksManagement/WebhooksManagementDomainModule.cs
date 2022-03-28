using LINGYUN.Abp.Webhooks;
using LINGYUN.Abp.WebhooksManagement.ObjectExtending;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.WebhooksManagement;

[DependsOn(
    typeof(AbpAutoMapperModule),
    typeof(AbpWebhooksModule),
    typeof(WebhooksManagementDomainSharedModule))]
public class WebhooksManagementDomainModule : AbpModule
{
    private static readonly OneTimeRunner OneTimeRunner = new();
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<WebhooksManagementDomainModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<WebhooksManagementDomainMapperProfile>(validate: true);
        });

        Configure<AbpDistributedEntityEventOptions>(options =>
        {
            options.EtoMappings.Add<WebhookEventRecord, WebhookEventEto>();
            options.EtoMappings.Add<WebhookSendRecord, WebhookSendAttemptEto>();
            options.EtoMappings.Add<WebhookSubscription, WebhookSubscriptionEto>();

            options.AutoEventSelectors.Add<WebhookEventRecord>();
            options.AutoEventSelectors.Add<WebhookSendRecord>();
            options.AutoEventSelectors.Add<WebhookSubscription>();
        });
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            // 扩展实体配置
            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                WebhooksManagementModuleExtensionConsts.ModuleName,
                WebhooksManagementModuleExtensionConsts.EntityNames.WebhookEvent,
                typeof(WebhookEventRecord)
            );
            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                WebhooksManagementModuleExtensionConsts.ModuleName,
                WebhooksManagementModuleExtensionConsts.EntityNames.WebhookSubscription,
                typeof(WebhookSubscription)
            );
            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                WebhooksManagementModuleExtensionConsts.ModuleName,
                WebhooksManagementModuleExtensionConsts.EntityNames.WebhookSendAttempt,
                typeof(WebhookSendRecord)
            );
        });
    }
}
