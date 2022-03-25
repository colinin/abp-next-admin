using LINGYUN.Abp.Webhooks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Modularity;
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
        });
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            // 扩展实体配置
            //ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
            //    WebhooksManagementModuleExtensionConsts.ModuleName,
            //    WebhooksManagementModuleExtensionConsts.EntityNames.Entity,
            //    typeof(Entity)
            //);
        });
    }
}
