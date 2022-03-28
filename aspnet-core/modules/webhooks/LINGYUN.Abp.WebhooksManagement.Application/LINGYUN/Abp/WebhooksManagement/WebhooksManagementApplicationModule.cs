using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.WebhooksManagement;

[DependsOn(
    typeof(AbpAuthorizationModule),
    typeof(AbpDddApplicationModule),
    typeof(WebhooksManagementDomainModule))]
public class WebhooksManagementApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<WebhooksManagementApplicationModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<WebhooksManagementApplicationMapperProfile>(validate: true);
        });
    }
}
