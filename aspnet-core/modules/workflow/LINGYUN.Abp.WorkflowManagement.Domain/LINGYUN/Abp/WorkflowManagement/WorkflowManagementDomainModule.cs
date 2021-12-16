using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.WorkflowManagement
{
    [DependsOn(
        typeof(AbpAutoMapperModule),
        typeof(WorkflowManagementDomainSharedModule))]
    public class WorkflowManagementDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<WorkflowManagementDomainModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<WorkflowManagementDomainMapperProfile>(validate: true);
            });

            Configure<AbpDistributedEntityEventOptions>(options =>
            {

            });

            context.Services.AddHostedService<WorkflowRegisterService>();
        }
    }
}
