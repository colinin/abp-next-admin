using LINGYUN.Abp.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using PackageName.CompanyName.ProjectName.ObjectExtending;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.Threading;

namespace PackageName.CompanyName.ProjectName;

[DependsOn(
    typeof(AbpAutoMapperModule),
    typeof(AbpDataProtectionModule),
    typeof(ProjectNameDomainSharedModule))]
public class ProjectNameDomainModule : AbpModule
{
    private static readonly OneTimeRunner OneTimeRunner = new();
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<ProjectNameDomainModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<ProjectNameDomainMapperProfile>(validate: true);
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
            //    ProjectNameModuleExtensionConsts.ModuleName,
            //    ProjectNameModuleExtensionConsts.EntityNames.Entity,
            //    typeof(Entity)
            //);
        });
    }
}
