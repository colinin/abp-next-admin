using LINGYUN.Abp.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Mapperly;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace PackageName.CompanyName.ProjectName;

[DependsOn(
    typeof(AbpMapperlyModule),
    typeof(AbpDataProtectionModule),
    typeof(ProjectNameDomainSharedModule))]
public class ProjectNameDomainModule : AbpModule
{
    private static readonly OneTimeRunner OneTimeRunner = new();
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMapperlyObjectMapper<ProjectNameDomainModule>();

        Configure<AbpDistributedEntityEventOptions>(options =>
        {
            // options.AutoEventSelectors.Add<Entity>();

            // options.EtoMappings.Add<Entity, EntityEto>();
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
